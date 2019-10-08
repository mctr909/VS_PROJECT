package com.lushprojects.circuitjs1.client;

import java.util.Vector;
import java.util.HashMap;
import java.util.Map;

public final class CircuitManager {
	private CirSim mCirSim;

	private double mCircuitMatrix[][];
	private double mCircuitRightSide[];
	private double mOrigRightSide[];
	private double mOrigMatrix[][];
	private RowInfo mCircuitRowInfo[];
	private int mCircuitPermute[];

	private CircuitElm mVoltageSources[];

	private boolean mSimRunning;
	private boolean mCircuitNonLinear;
	private boolean mCircuitNeedsMap;
	private boolean mDumpMatrix;

	private int mVoltageSourceCount;
	private int mCircuitMatrixSize;
	private int mCircuitMatrixFullSize;

	private int mScopeColCount[];

	// map points to node numbers
	class NodeMapEntry {
		int node;
		NodeMapEntry() {
			node = -1;
		}
		NodeMapEntry(int n) {
			node = n;
		}
	}
	private HashMap<Point, NodeMapEntry> mNodeMap;
	private HashMap<Point, Integer> mPostCountMap;

	// info about each wire and its neighbors, used to calculate wire currents
	class WireInfo {
		WireElm wire;
		Vector<CircuitElm> neighbors;
		int post;
		WireInfo(WireElm w) {
			wire = w;
		}
	}
	private Vector<WireInfo> mWireInfoList;

	class FindPathInfo {
		static final int INDUCT = 1;
		static final int VOLTAGE = 2;
		static final int SHORT = 3;
		static final int CAP_V = 4;
		boolean used[];
		int dest;
		CircuitElm firstElm;
		int type;

		FindPathInfo(int t, CircuitElm e, int d) {
			dest = d;
			type = t;
			firstElm = e;
			used = new boolean[mNodeList.size()];
		}

		boolean findPath(int n1) {
			return findPath(n1, -1);
		}

		boolean findPath(int n1, int depth) {
			if (n1 == dest) return true;
			if (depth-- == 0) return false;
			if (used[n1]) {
				// System.out.println("used " + n1);
				return false;
			}
			used[n1] = true;
			int i;
			for (i = 0; i != mCirSim.mElmList.size(); i++) {
				CircuitElm ce = mCirSim.getElm(i);
				if (ce == firstElm) continue;
				if (type == INDUCT) {
					// inductors need a path free of current sources
					if (ce instanceof CurrentElm) continue;
				}
				if (type == VOLTAGE) {
					// when checking for voltage loops, we only care about voltage
					// sources/wires/ground
					if (!(ce.isWire() || ce instanceof VoltageElm || ce instanceof GroundElm)) continue;
				}
				// when checking for shorts, just check wires
				if (type == SHORT && !ce.isWire()) continue;
				if (type == CAP_V) {
					// checking for capacitor/voltage source loops
					if (!(ce.isWire() || ce instanceof CapacitorElm || ce instanceof VoltageElm)) continue;
				}
				if (n1 == 0) {
					// look for posts which have a ground connection;
					// our path can go through ground
					int j;
					for (j = 0; j != ce.getConnectionNodeCount(); j++) {
						if (ce.hasGroundConnection(j) && findPath(ce.getConnectionNode(j), depth)) {
							used[n1] = false;
							return true;
						}
					}
				}
				int j;
				for (j = 0; j != ce.getConnectionNodeCount(); j++) {
					// System.out.println(ce + " " + ce.getNode(j));
					if (ce.getConnectionNode(j) == n1) break;
				}
				if (j == ce.getConnectionNodeCount()) continue;
				if (ce.hasGroundConnection(j) && findPath(0, depth)) {
					// System.out.println(ce + " has ground");
					used[n1] = false;
					return true;
				}
				if (type == INDUCT && ce instanceof InductorElm) {
					// inductors can use paths with other inductors of matching current
					double c = ce.getCurrent();
					if (j == 0) {
						c = -c;
					}
					// System.out.println("matching " + c + " to " + firstElm.getCurrent());
					// System.out.println(ce + " " + firstElm);
					if (Math.abs(c - firstElm.getCurrent()) > 1e-10) continue;
				}
				int k;
				for (k = 0; k != ce.getConnectionNodeCount(); k++) {
					if (j == k) continue;
					// console(ce + " " + ce.getNode(j) + "-" + ce.getNode(k));
					if (ce.getConnection(j, k) && findPath(ce.getConnectionNode(k), depth)) {
						// System.out.println("got findpath " + n1);
						used[n1] = false;
						return true;
					}
					// System.out.println("back on findpath " + n1);
				}
			}
			used[n1] = false;
			// System.out.println(n1 + " failed");
			return false;
		}
	}

	public Scope mScopes[];
	public int mScopeCount;

	public boolean mAnalyzeFlag;
	public boolean mConverged;
	public boolean mShowResistanceInVoltageSources;
	public int mSubIterations;

	public double mTime;
	public double mDeltaTime;
	public long mSecTime = 0;
	public int mSteps = 0;
	public int mFrames = 0;
	public int mFramerate = 0;
	public long mLastTime = 0;
	public long mLastFrameTime;
	public long mLastIterTime;
	public int mSteprate = 0;

	public Vector<CircuitNode> mNodeList;
	public Vector<Point> mPostDrawList = new Vector<Point>();
	public Vector<Point> mBadConnectionList = new Vector<Point>();
	public CircuitElm mErrorElm;

	public CircuitManager(CirSim cirSim) {
		mCirSim = cirSim;
		mScopes = new Scope[20];
		mScopeCount = 0;
		mScopeColCount = new int[20];
	}

	public boolean simIsRunning() {
		return mSimRunning;
	}

	public void simStop() {
		mSimRunning = false;
	}

	public void simStart() {
		mSimRunning = false;
	}

	public void resetAction() {
		int i;
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			mCirSim.getElm(i).reset();
		}
		for (i = 0; i != mScopeCount; i++) {
			mScopes[i].resetGraph(true);
		}
		// TODO: Will need to do IE bug fix here?
		mAnalyzeFlag = true;
		if (mTime == 0) {
			mCirSim.setSimRunning(true);
		} else {
			mTime = 0;
		}
	}

	public void stop(String s, CircuitElm ce) {
		mCirSim.stopMessage = CirSim.LS(s);
		mCircuitMatrix = null; // causes an exception
		mErrorElm = ce;
		mCirSim.setSimRunning(false);
		mAnalyzeFlag = false;
		// cv.repaint();
	}

	public void runCircuit(boolean didAnalyze) {
		if (null == mCircuitMatrix || 0 == mCirSim.mElmList.size()) {
			mCircuitMatrix = null;
			return;
		}
		int iter;
		// int maxIter = getIterCount();
		boolean debugprint = mDumpMatrix;
		mDumpMatrix = false;
		long steprate = (long) (160 * mCirSim.getIterCount());
		long tm = System.currentTimeMillis();
		long lit = mLastIterTime;
		if (0 == lit) {
			mLastIterTime = tm;
			return;
		}

		// Check if we don't need to run simulation (for very slow simulation speeds).
		// If the circuit changed, do at least one iteration to make sure everything is
		// consistent.
		if (1000 >= steprate * (tm - mLastIterTime) && !didAnalyze) return;

		boolean delayWireProcessing = canDelayWireProcessing();

		for (iter = 1;; iter++) {
			int i, j, k, subiter;
			for (i = 0; i != mCirSim.mElmList.size(); i++) {
				CircuitElm ce = mCirSim.getElm(i);
				ce.startIteration();
			}
			mSteps++;
			final int subiterCount = 5000;
			for (subiter = 0; subiter != subiterCount; subiter++) {
				mConverged = true;
				mSubIterations = subiter;
				for (i = 0; i != mCircuitMatrixSize; i++) {
					mCircuitRightSide[i] = mOrigRightSide[i];
				}
				if (mCircuitNonLinear) {
					for (i = 0; i != mCircuitMatrixSize; i++) {
						for (j = 0; j != mCircuitMatrixSize; j++) {
							mCircuitMatrix[i][j] = mOrigMatrix[i][j];
						}
					}
				}
				for (i = 0; i != mCirSim.mElmList.size(); i++) {
					CircuitElm ce = mCirSim.getElm(i);
					ce.doStep();
				}
				if (null != mCirSim.stopMessage) return;
				boolean printit = debugprint;
				debugprint = false;
				for (j = 0; j != mCircuitMatrixSize; j++) {
					for (i = 0; i != mCircuitMatrixSize; i++) {
						double x = mCircuitMatrix[i][j];
						if (Double.isNaN(x) || Double.isInfinite(x)) {
							stop("nan/infinite matrix!", null);
							return;
						}
					}
				}
				if (printit) {
					for (j = 0; j != mCircuitMatrixSize; j++) {
						String x = "";
						for (i = 0; i != mCircuitMatrixSize; i++) {
							x += mCircuitMatrix[j][i] + ",";
						}
						x += "\n";
						CirSim.console(x);
					}
					CirSim.console("");
				}
				if (mCircuitNonLinear) {
					if (mConverged && subiter > 0) break;
					if (!lu_factor(mCircuitMatrix, mCircuitMatrixSize, mCircuitPermute)) {
						stop("Singular matrix!", null);
						return;
					}
				}
				lu_solve(mCircuitMatrix, mCircuitMatrixSize, mCircuitPermute, mCircuitRightSide);

				for (j = 0; j != mCircuitMatrixFullSize; j++) {
					RowInfo ri = mCircuitRowInfo[j];
					double res = 0;
					if (ri.type == RowInfo.ROW_CONST) {
						res = ri.value;
					} else {
						res = mCircuitRightSide[ri.mapCol];
					}
					/*
					 * System.out.println(j + " " + res + " " + ri.type + " " + ri.mapCol);
					 */
					if (Double.isNaN(res)) {
						mConverged = false;
						// debugprint = true;
						break;
					}
					if (j < mNodeList.size() - 1) {
						CircuitNode cn = getCircuitNode(j + 1);
						for (k = 0; k != cn.links.size(); k++) {
							CircuitNodeLink cnl = (CircuitNodeLink) cn.links.elementAt(k);
							cnl.elm.setNodeVoltage(cnl.num, res);
						}
					} else {
						int ji = j - (mNodeList.size() - 1);
						// System.out.println("setting vsrc " + ji + " to " + res);
						mVoltageSources[ji].setCurrent(ji, res);
					}
				}
				if (!mCircuitNonLinear) break;
			}
			if (subiter > 5) {
				CirSim.console("converged after " + subiter + " iterations\n");
			}
			if (subiter == subiterCount) {
				stop("Convergence failed!", null);
				break;
			}
			mTime += mDeltaTime;
			for (i = 0; i != mCirSim.mElmList.size(); i++) {
				mCirSim.getElm(i).stepFinished();
			}
			if (!delayWireProcessing) {
				calcWireCurrents();
			}
			for (i = 0; i != mScopeCount; i++) {
				mScopes[i].timeStep();
			}
			for (i = 0; i != mCirSim.mElmList.size(); i++) {
				if (mCirSim.getElm(i) instanceof ScopeElm) {
					((ScopeElm) mCirSim.getElm(i)).stepScope();
				}
			}

			tm = System.currentTimeMillis();
			lit = tm;
			// Check whether enough time has elapsed to perform an *additional* iteration
			// after
			// those we have already completed.
			if ((iter + 1) * 1000 >= steprate * (tm - mLastIterTime) || (tm - mLastFrameTime > 500)) {
				break;
			}
			if (!mSimRunning) {
				break;
			}
		} // for (iter = 1; ; iter++)
		mLastIterTime = lit;
		if (delayWireProcessing) {
			calcWireCurrents();
		}
		// System.out.println((System.currentTimeMillis()-mLastFrameTime)/(double) iter);
	}

	public void analyzeCircuit() {
		if (mCirSim.mElmList.isEmpty()) {
			mPostDrawList = new Vector<Point>();
			mBadConnectionList = new Vector<Point>();
			return;
		}
		mCirSim.stopMessage = null;
		mErrorElm = null;
		int i, j;
		int vscount = 0;
		mNodeList = new Vector<CircuitNode>();
		mPostCountMap = new HashMap<Point, Integer>();
		boolean gotGround = false;
		boolean gotRail = false;
		CircuitElm volt = null;

		calculateWireClosure();

		// System.out.println("ac1");
		// look for voltage or ground element
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			if (ce instanceof GroundElm) {
				gotGround = true;
				break;
			}
			if (ce instanceof RailElm) {
				gotRail = true;
			}
			if (volt == null && ce instanceof VoltageElm) {
				volt = ce;
			}
		}

		// if no ground, and no rails, then the voltage elm's first terminal
		// is ground
		if (!gotGround && volt != null && !gotRail) {
			CircuitNode cn = new CircuitNode();
			Point pt = volt.getPost(0);
			mNodeList.addElement(cn);

			// update node map
			NodeMapEntry cln = mNodeMap.get(pt);
			if (cln != null) {
				cln.node = 0;
			} else {
				mNodeMap.put(pt, new NodeMapEntry(0));
			}
		} else {
			// otherwise allocate extra node for ground
			CircuitNode cn = new CircuitNode();
			mNodeList.addElement(cn);
		}
		// System.out.println("ac2");

		// allocate nodes and voltage sources
		LabeledNodeElm.resetNodeList();
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			int inodes = ce.getInternalNodeCount();
			int ivs = ce.getVoltageSourceCount();
			int posts = ce.getPostCount();

			// allocate a node for each post and match posts to nodes
			for (j = 0; j != posts; j++) {
				Point pt = ce.getPost(j);
				Integer g = mPostCountMap.get(pt);
				mPostCountMap.put(pt, g == null ? 1 : g + 1);
				NodeMapEntry cln = mNodeMap.get(pt);

				// is this node not in map yet? or is the node number unallocated?
				// (we don't allocate nodes before this because changing the allocation order
				// of nodes changes circuit behavior and breaks backward compatibility;
				// the code below to connect unconnected nodes may connect a different node to
				// ground)
				if (cln == null || cln.node == -1) {
					CircuitNode cn = new CircuitNode();
					CircuitNodeLink cnl = new CircuitNodeLink();
					cnl.num = j;
					cnl.elm = ce;
					cn.links.addElement(cnl);
					ce.setNode(j, mNodeList.size());
					if (cln != null) {
						cln.node = mNodeList.size();
					} else {
						mNodeMap.put(pt, new NodeMapEntry(mNodeList.size()));
					}
					mNodeList.addElement(cn);
				} else {
					int n = cln.node;
					CircuitNodeLink cnl = new CircuitNodeLink();
					cnl.num = j;
					cnl.elm = ce;
					getCircuitNode(n).links.addElement(cnl);
					ce.setNode(j, n);
					// if it's the ground node, make sure the node voltage is 0,
					// cause it may not get set later
					if (n == 0) {
						ce.setNodeVoltage(j, 0);
					}
				}
			}
			for (j = 0; j != inodes; j++) {
				CircuitNode cn = new CircuitNode();
				cn.internal = true;
				CircuitNodeLink cnl = new CircuitNodeLink();
				cnl.num = j + posts;
				cnl.elm = ce;
				cn.links.addElement(cnl);
				ce.setNode(cnl.num, mNodeList.size());
				mNodeList.addElement(cn);
			}
			vscount += ivs;
		}

		makePostDrawList();
		if (!calcWireInfo()) return;
		mNodeMap = null; // done with this

		mVoltageSources = new CircuitElm[vscount];
		vscount = 0;
		mCircuitNonLinear = false;
		// System.out.println("ac3");

		// determine if circuit is nonlinear
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			if (ce.nonLinear()) {
				mCircuitNonLinear = true;
			}
			int ivs = ce.getVoltageSourceCount();
			for (j = 0; j != ivs; j++) {
				mVoltageSources[vscount] = ce;
				ce.setVoltageSource(j, vscount++);
			}
		}
		mVoltageSourceCount = vscount;

		int matrixSize = mNodeList.size() - 1 + vscount;
		mCircuitMatrix = new double[matrixSize][matrixSize];
		mCircuitRightSide = new double[matrixSize];
		mOrigMatrix = new double[matrixSize][matrixSize];
		mOrigRightSide = new double[matrixSize];
		mCircuitMatrixSize = mCircuitMatrixFullSize = matrixSize;
		mCircuitRowInfo = new RowInfo[matrixSize];
		mCircuitPermute = new int[matrixSize];
		for (i = 0; i != matrixSize; i++) {
			mCircuitRowInfo[i] = new RowInfo();
		}
		mCircuitNeedsMap = false;

		// stamp linear circuit elements
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			ce.stamp();
		}
		// System.out.println("ac4");

		// determine nodes that are not connected indirectly to ground
		boolean closure[] = new boolean[mNodeList.size()];
		boolean changed = true;
		closure[0] = true;
		while (changed) {
			changed = false;
			for (i = 0; i != mCirSim.mElmList.size(); i++) {
				CircuitElm ce = mCirSim.getElm(i);
				if (ce instanceof WireElm) continue;
				// loop through all ce's nodes to see if they are connected
				// to other nodes not in closure
				for (j = 0; j < ce.getConnectionNodeCount(); j++) {
					if (!closure[ce.getConnectionNode(j)]) {
						if (ce.hasGroundConnection(j)) {
							closure[ce.getConnectionNode(j)] = changed = true;
						}
						continue;
					}
					int k;
					for (k = 0; k != ce.getConnectionNodeCount(); k++) {
						if (j == k) continue;
						int kn = ce.getConnectionNode(k);
						if (ce.getConnection(j, k) && !closure[kn]) {
							closure[kn] = true;
							changed = true;
						}
					}
				}
			}
			if (changed) continue;

			// connect one of the unconnected nodes to ground with a big resistor, then try
			// again
			for (i = 0; i != mNodeList.size(); i++) {
				if (!closure[i] && !getCircuitNode(i).internal) {
					CirSim.console("node " + i + " unconnected");
					stampResistor(0, i, 1e8);
					closure[i] = true;
					changed = true;
					break;
				}
			}
		}
		// System.out.println("ac5");

		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			// look for inductors with no current path
			if (ce instanceof InductorElm) {
				FindPathInfo fpi = new FindPathInfo(FindPathInfo.INDUCT, ce, ce.getNode(1));
				// first try findPath with maximum depth of 5, to avoid slowdowns
				if (!fpi.findPath(ce.getNode(0), 5) && !fpi.findPath(ce.getNode(0))) {
					// console(ce + " no path");
					ce.reset();
				}
			}
			// look for current sources with no current path
			if (ce instanceof CurrentElm) {
				CurrentElm cur = (CurrentElm) ce;
				FindPathInfo fpi = new FindPathInfo(FindPathInfo.INDUCT, ce, ce.getNode(1));
				// first try findPath with maximum depth of 5, to avoid slowdowns
				if (!fpi.findPath(ce.getNode(0), 5) && !fpi.findPath(ce.getNode(0))) {
					cur.stampCurrentSource(true);
				} else {
					cur.stampCurrentSource(false);
				}
			}
			if (ce instanceof VCCSElm) {
				VCCSElm cur = (VCCSElm) ce;
				FindPathInfo fpi = new FindPathInfo(FindPathInfo.INDUCT, ce, cur.getOutputNode(0));
				if (cur.hasCurrentOutput() && !fpi.findPath(cur.getOutputNode(1))) {
					cur.broken = true;
				} else {
					cur.broken = false;
				}
			}
			// look for voltage source or wire loops. we do this for voltage sources or
			// wire-like elements (not actual wires
			// because those are optimized out, so the findPath won't work)
			if (ce.getPostCount() == 2) {
				if (ce instanceof VoltageElm || (ce.isWire() && !(ce instanceof WireElm))) {
					FindPathInfo fpi = new FindPathInfo(FindPathInfo.VOLTAGE, ce, ce.getNode(1));
					if (fpi.findPath(ce.getNode(0))) {
						stop("Voltage source/wire loop with no resistance!", ce);
						return;
					}
				}
			}

			// look for path from rail to ground
			if (ce instanceof RailElm) {
				FindPathInfo fpi = new FindPathInfo(FindPathInfo.VOLTAGE, ce, ce.getNode(0));
				if (fpi.findPath(0)) {
					stop("Path to ground with no resistance!", ce);
					return;
				}
			}

			// look for shorted caps, or caps w/ voltage but no R
			if (ce instanceof CapacitorElm) {
				FindPathInfo fpi = new FindPathInfo(FindPathInfo.SHORT, ce, ce.getNode(1));
				if (fpi.findPath(ce.getNode(0))) {
					CirSim.console(ce + " shorted");
					ce.reset();
				} else {
					// a capacitor loop used to cause a matrix error. but we changed the capacitor
					// model
					// so it works fine now. The only issue is if a capacitor is added in parallel
					// with
					// another capacitor with a nonzero voltage; in that case we will get
					// oscillation unless
					// we reset both capacitors to have the same voltage. Rather than check for
					// that, we just
					// give an error.
					fpi = new FindPathInfo(FindPathInfo.CAP_V, ce, ce.getNode(1));
					if (fpi.findPath(ce.getNode(0))) {
						stop("Capacitor loop with no resistance!", ce);
						return;
					}
				}
			}
		}
		// System.out.println("ac6");

		if (!simplifyMatrix(matrixSize)) return;

		/*
		 * System.out.println("matrixSize = " + matrixSize + " " + circuitNonLinear);
		 * for (j = 0; j != circuitMatrixSize; j++) { for (i = 0; i !=
		 * circuitMatrixSize; i++) System.out.print(circuitMatrix[j][i] + " ");
		 * System.out.print("  " + circuitRightSide[j] + "\n"); }
		 * System.out.print("\n");
		 */

		// check if we called stop()
		if (mCircuitMatrix == null) return;

		// if a matrix is linear, we can do the lu_factor here instead of
		// needing to do it every frame
		if (!mCircuitNonLinear) {
			if (!lu_factor(mCircuitMatrix, mCircuitMatrixSize, mCircuitPermute)) {
				stop("Singular matrix!", null);
				return;
			}
		}

		// show resistance in voltage sources if there's only one
		boolean gotVoltageSource = false;
		mShowResistanceInVoltageSources = true;
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			if (ce instanceof VoltageElm) {
				if (gotVoltageSource) {
					mShowResistanceInVoltageSources = false;
				} else {
					gotVoltageSource = true;
				}
			}
		}
	}

	public void setupScopes() {
		int i;

		// check scopes to make sure the elements still exist, and remove
		// unused scopes/columns
		int pos = -1;
		for (i = 0; i < mScopeCount; i++) {
			if (mScopes[i].needToRemove()) {
				int j;
				for (j = i; j != mScopeCount; j++) {
					mScopes[j] = mScopes[j + 1];
				}
				mScopeCount--;
				i--;
				continue;
			}
			if (mScopes[i].position > pos + 1) {
				mScopes[i].position = pos + 1;
			}
			pos = mScopes[i].position;
		}
		while (mScopeCount > 0 && mScopes[mScopeCount - 1].getElm() == null) {
			mScopeCount--;
		}
		int h = mCirSim.cv.getCoordinateSpaceHeight() - mCirSim.circuitArea.height;
		pos = 0;
		for (i = 0; i != mScopeCount; i++) {
			mScopeColCount[i] = 0;
		}
		for (i = 0; i != mScopeCount; i++) {
			pos = CirSim.max(mScopes[i].position, pos);
			mScopeColCount[mScopes[i].position]++;
		}
		int colct = pos + 1;
		int iw = CirSim.infoWidth;
		if (colct <= 2) {
			iw = iw * 3 / 2;
		}
		int w = (mCirSim.cv.getCoordinateSpaceWidth() - iw) / colct;
		int marg = 10;
		if (w < marg * 2) {
			w = marg * 2;
		}
		pos = -1;
		int colh = 0;
		int row = 0;
		int speed = 0;
		for (i = 0; i != mScopeCount; i++) {
			Scope s = mScopes[i];
			if (s.position > pos) {
				pos = s.position;
				colh = h / mScopeColCount[pos];
				row = 0;
				speed = s.speed;
			}
			s.stackCount = mScopeColCount[pos];
			if (s.speed != speed) {
				s.speed = speed;
				s.resetGraph();
			}
			Rectangle r = new Rectangle(pos * w, mCirSim.cv.getCoordinateSpaceHeight() - h + colh * row, w - marg, colh);
			row++;
			if (!r.equals(s.rect)) {
				s.setRect(r);
			}
		}
	}

	public void addScope() {
		int i;
		for (i = 0; i != mScopeCount; i++) {
			if (mScopes[i].getElm() == null) break;
		}
		if (i == mScopeCount) {
			if (mScopeCount == mScopes.length) return;
			mScopeCount++;
			mScopes[i] = new Scope(mCirSim);
			mScopes[i].position = i;
			// handleResize();
		}
		mScopes[i].setElm(mCirSim.menuElm);
		if (i > 0) {
			mScopes[i].speed = mScopes[i - 1].speed;
		}
	}

	public void stackScope(int s) {
		if (s == 0) {
			if (mScopeCount < 2) return;
			s = 1;
		}
		if (mScopes[s].position == mScopes[s - 1].position) return;
		mScopes[s].position = mScopes[s - 1].position;
		for (s++; s < mScopeCount; s++) {
			mScopes[s].position--;
		}
	}

	public void unstackScope(int s) {
		if (s == 0) {
			if (mScopeCount < 2) return;
			s = 1;
		}
		if (mScopes[s].position != mScopes[s - 1].position) return;
		for (; s < mScopeCount; s++) {
			mScopes[s].position++;
		}
	}

	public void combineScope(int s) {
		if (s == 0) {
			if (mScopeCount < 2) return;
			s = 1;
		}
		mScopes[s - 1].combine(mScopes[s]);
		mScopes[s].setElm(null);
	}

	public void stackAll() {
		int i;
		for (i = 0; i != mScopeCount; i++) {
			mScopes[i].position = 0;
			mScopes[i].showMax = mScopes[i].showMin = false;
		}
	}

	public void unstackAll() {
		int i;
		for (i = 0; i != mScopeCount; i++) {
			mScopes[i].position = i;
			mScopes[i].showMax = true;
		}
	}

	public void combineAll() {
		int i;
		for (i = mScopeCount - 2; i >= 0; i--) {
			mScopes[i].combine(mScopes[i + 1]);
			mScopes[i + 1].setElm(null);
		}
	}

	// update voltage source in doStep()
	public void updateVoltageSource(int n1, int n2, int vs, double v) {
		int vn = mNodeList.size() + vs;
		stampRightSide(vn, v);
	}

	// control voltage source vs with voltage from n1 to n2 (must
	// also call stampVoltageSource())
	public void stampVCVS(int n1, int n2, double coef, int vs) {
		int vn = mNodeList.size() + vs;
		stampMatrix(vn, n1, coef);
		stampMatrix(vn, n2, -coef);
	}

	// stamp independent voltage source #vs, from n1 to n2, amount v
	public void stampVoltageSource(int n1, int n2, int vs, double v) {
		int vn = mNodeList.size() + vs;
		stampMatrix(vn, n1, -1);
		stampMatrix(vn, n2, 1);
		stampRightSide(vn, v);
		stampMatrix(n1, vn, 1);
		stampMatrix(n2, vn, -1);
	}

	// use this if the amount of voltage is going to be updated in doStep(), by
	// updateVoltageSource()
	public void stampVoltageSource(int n1, int n2, int vs) {
		int vn = mNodeList.size() + vs;
		stampMatrix(vn, n1, -1);
		stampMatrix(vn, n2, 1);
		stampRightSide(vn);
		stampMatrix(n1, vn, 1);
		stampMatrix(n2, vn, -1);
	}

	public void stampResistor(int n1, int n2, double r) {
		double r0 = 1 / r;
		if (Double.isNaN(r0) || Double.isInfinite(r0)) {
			System.out.print("bad resistance " + r + " " + r0 + "\n");
			int a = 0;
			a /= a;
		}
		stampMatrix(n1, n1, r0);
		stampMatrix(n2, n2, r0);
		stampMatrix(n1, n2, -r0);
		stampMatrix(n2, n1, -r0);
	}

	public void stampConductance(int n1, int n2, double r0) {
		stampMatrix(n1, n1, r0);
		stampMatrix(n2, n2, r0);
		stampMatrix(n1, n2, -r0);
		stampMatrix(n2, n1, -r0);
	}

	// current from cn1 to cn2 is equal to voltage from vn1 to 2, divided by g
	public void stampVCCurrentSource(int cn1, int cn2, int vn1, int vn2, double g) {
		stampMatrix(cn1, vn1, g);
		stampMatrix(cn2, vn2, g);
		stampMatrix(cn1, vn2, -g);
		stampMatrix(cn2, vn1, -g);
	}

	public void stampCurrentSource(int n1, int n2, double i) {
		stampRightSide(n1, -i);
		stampRightSide(n2, i);
	}

	// stamp a current source from n1 to n2 depending on current through vs
	public void stampCCCS(int n1, int n2, int vs, double gain) {
		int vn = mNodeList.size() + vs;
		stampMatrix(n1, vn, gain);
		stampMatrix(n2, vn, -gain);
	}

	// indicate that the values on the left side of row i change in doStep()
	public void stampNonLinear(int i) {
		if (i > 0) {
			mCircuitRowInfo[i - 1].lsChanges = true;
		}
	}

	// stamp value x in row i, column j, meaning that a voltage change
	// of dv in node j will increase the current into node i by x dv.
	// (Unless i or j is a voltage source node.)
	public void stampMatrix(int i, int j, double x) {
		if (i > 0 && j > 0) {
			if (mCircuitNeedsMap) {
				i = mCircuitRowInfo[i - 1].mapRow;
				RowInfo ri = mCircuitRowInfo[j - 1];
				if (ri.type == RowInfo.ROW_CONST) {
					// System.out.println("Stamping constant " + i + " " + j + " " + x);
					mCircuitRightSide[i] -= x * ri.value;
					return;
				}
				j = ri.mapCol;
				// System.out.println("stamping " + i + " " + j + " " + x);
			} else {
				i--;
				j--;
			}
			mCircuitMatrix[i][j] += x;
		}
	}

	// stamp value x on the right side of row i, representing an
	// independent current source flowing into node i
	public void stampRightSide(int i, double x) {
		if (i > 0) {
			if (mCircuitNeedsMap) {
				i = mCircuitRowInfo[i - 1].mapRow;
				// System.out.println("stamping " + i + " " + x);
			} else {
				i--;
			}
			mCircuitRightSide[i] += x;
		}
	}

	// indicate that the value on the right side of row i changes in doStep()
	public void stampRightSide(int i) {
		// System.out.println("rschanges true " + (i-1));
		if (i > 0) {
			mCircuitRowInfo[i - 1].rsChanges = true;
		}
	}

	// find groups of nodes connected by wires and map them to the same node. this
	// speeds things
	// up considerably by reducing the size of the matrix
	private void calculateWireClosure() {
		int i;
		mNodeMap = new HashMap<Point, NodeMapEntry>();
		// int mergeCount = 0;
		mWireInfoList = new Vector<WireInfo>();
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			CircuitElm ce = mCirSim.getElm(i);
			if (!(ce instanceof WireElm)) continue;
			WireElm we = (WireElm) ce;
			we.hasWireInfo = false;
			mWireInfoList.add(new WireInfo(we));
			NodeMapEntry cn = mNodeMap.get(ce.getPost(0));
			NodeMapEntry cn2 = mNodeMap.get(ce.getPost(1));
			if (cn != null && cn2 != null) {
				// merge nodes; go through map and change all keys pointing to cn2 to point to
				// cn
				for (Map.Entry<Point, NodeMapEntry> entry : mNodeMap.entrySet()) {
					if (entry.getValue() == cn2) {
						entry.setValue(cn);
					}
				}
				// mergeCount++;
				continue;
			}
			if (cn != null) {
				mNodeMap.put(ce.getPost(1), cn);
				continue;
			}
			if (cn2 != null) {
				mNodeMap.put(ce.getPost(0), cn2);
				continue;
			}
			// new entry
			cn = new NodeMapEntry();
			mNodeMap.put(ce.getPost(0), cn);
			mNodeMap.put(ce.getPost(1), cn);
		}

		// console("got " + (groupCount-mergeCount) + " groups with " + nodeMap.size() +
		// " nodes " + mergeCount);
	}

	// make list of posts we need to draw. posts shared by 2 elements should be
	// hidden, all
	// others should be drawn. We can't use the node list anymore because wires have
	// the same
	// node number at both ends.
	private void makePostDrawList() {
		mPostDrawList = new Vector<Point>();
		mBadConnectionList = new Vector<Point>();
		for (Map.Entry<Point, Integer> entry : mPostCountMap.entrySet()) {
			if (entry.getValue() != 2) {
				mPostDrawList.add(entry.getKey());
			}

			// look for bad connections, posts not connected to other elements which
			// intersect
			// other elements' bounding boxes
			if (entry.getValue() == 1) {
				int j;
				boolean bad = false;
				Point cn = entry.getKey();
				for (j = 0; j != mCirSim.mElmList.size() && !bad; j++) {
					CircuitElm ce = mCirSim.getElm(j);
					if (ce instanceof GraphicElm) continue;
					// does this post intersect elm's bounding box?
					if (!ce.boundingBox.contains(cn.x, cn.y)) continue;
					int k;
					// does this post belong to the elm?
					int pc = ce.getPostCount();
					for (k = 0; k != pc; k++) {
						if (ce.getPost(k).equals(cn)) {
							break;
						}
					}
					if (k == pc) {
						bad = true;
					}
				}
				if (bad) {
					mBadConnectionList.add(cn);
				}
			}
		}
		mPostCountMap = null;
	}

	// generate info we need to calculate wire currents. Most other elements
	// calculate currents using
	// the voltage on their terminal nodes. But wires have the same voltage at both
	// ends, so we need
	// to use the neighbors' currents instead. We used to treat wires as zero
	// voltage sources to make
	// this easier, but this is very inefficient, since it makes the matrix 2 rows
	// bigger for each wire.
	// So we create a list of WireInfo objects instead to help us calculate the wire
	// currents instead,
	// so we make the matrix less complex, and we only calculate the wire currents
	// when we need them
	// (once per frame, not once per subiteration)
	private boolean calcWireInfo() {
		int i;
		int moved = 0;
		for (i = 0; i != mWireInfoList.size(); i++) {
			WireInfo wi = mWireInfoList.get(i);
			WireElm wire = wi.wire;
			CircuitNode cn1 = mNodeList.get(wire.getNode(0)); // both ends of wire have same node #
			int j;

			Vector<CircuitElm> neighbors0 = new Vector<CircuitElm>();
			Vector<CircuitElm> neighbors1 = new Vector<CircuitElm>();
			boolean isReady0 = true, isReady1 = true;

			// go through elements sharing a node with this wire (may be connected
			// indirectly
			// by other wires, but at least it's faster than going through all elements)
			for (j = 0; j != cn1.links.size(); j++) {
				CircuitNodeLink cnl = cn1.links.get(j);
				CircuitElm ce = cnl.elm;
				if (ce == wire) continue;
				Point pt = cnl.elm.getPost(cnl.num);

				// is this a wire that doesn't have wire info yet? If so we can't use it.
				// That would create a circular dependency
				boolean notReady = (ce instanceof WireElm && !((WireElm) ce).hasWireInfo);

				// which post does this element connect to, if any?
				if (pt.x == wire.x && pt.y == wire.y) {
					neighbors0.add(ce);
					if (notReady)
						isReady0 = false;
				} else if (pt.x == wire.x2 && pt.y == wire.y2) {
					neighbors1.add(ce);
					if (notReady)
						isReady1 = false;
				}
			}

			// does one of the posts have all information necessary to calculate current
			if (isReady0) {
				wi.neighbors = neighbors0;
				wi.post = 0;
				wire.hasWireInfo = true;
				moved = 0;
			} else if (isReady1) {
				wi.neighbors = neighbors1;
				wi.post = 1;
				wire.hasWireInfo = true;
				moved = 0;
			} else {
				// move to the end of the list and try again later
				mWireInfoList.add(mWireInfoList.remove(i--));
				moved++;
				if (moved > mWireInfoList.size() * 2) {
					stop("wire loop detected", wire);
					return false;
				}
			}
		}

		return true;
	}

	// simplify the matrix; this speeds things up quite a bit, especially for
	// digital circuits
	private boolean simplifyMatrix(int matrixSize) {
		int i, j;
		for (i = 0; i != matrixSize; i++) {
			int qp = -1;
			double qv = 0;
			RowInfo re = mCircuitRowInfo[i];
			/*
			 * System.out.println("row " + i + " " + re.lsChanges + " " + re.rsChanges + " "
			 * + re.dropRow);
			 */
			// if (qp != -100) continue; // uncomment to disable matrix simplification
			if (re.lsChanges || re.dropRow || re.rsChanges) continue;
			double rsadd = 0;

			// look for rows that can be removed
			for (j = 0; j != matrixSize; j++) {
				double q = mCircuitMatrix[i][j];
				if (mCircuitRowInfo[j].type == RowInfo.ROW_CONST) {
					// keep a running total of const values that have been
					// removed already
					rsadd -= mCircuitRowInfo[j].value * q;
					continue;
				}
				// ignore zeroes
				if (q == 0) continue;
				// keep track of first nonzero element that is not ROW_CONST
				if (qp == -1) {
					qp = j;
					qv = q;
					continue;
				}
				// more than one nonzero element? give up
				break;
			}
			if (j == matrixSize) {
				if (qp == -1) {
					// probably a singular matrix, try disabling matrix simplification above to
					// check this
					stop("Matrix error", null);
					return false;
				}
				RowInfo elt = mCircuitRowInfo[qp];
				// we found a row with only one nonzero nonconst entry; that value
				// is a constant
				if (elt.type != RowInfo.ROW_NORMAL) {
					System.out.println("type already " + elt.type + " for " + qp + "!");
					continue;
				}
				elt.type = RowInfo.ROW_CONST;
				// console("ROW_CONST " + i + " " + rsadd);
				elt.value = (mCircuitRightSide[i] + rsadd) / qv;
				mCircuitRowInfo[i].dropRow = true;
				i = -1; // start over from scratch
			}
		}
		// System.out.println("ac7");

		// find size of new matrix
		int nn = 0;
		for (i = 0; i != matrixSize; i++) {
			RowInfo elt = mCircuitRowInfo[i];
			if (elt.type == RowInfo.ROW_NORMAL) {
				elt.mapCol = nn++;
				// System.out.println("col " + i + " maps to " + elt.mapCol);
				continue;
			}
			if (elt.type == RowInfo.ROW_CONST)
				elt.mapCol = -1;
		}

		// make the new, simplified matrix
		int newsize = nn;
		double newmatx[][] = new double[newsize][newsize];
		double newrs[] = new double[newsize];
		int ii = 0;
		for (i = 0; i != matrixSize; i++) {
			RowInfo rri = mCircuitRowInfo[i];
			if (rri.dropRow) {
				rri.mapRow = -1;
				continue;
			}
			newrs[ii] = mCircuitRightSide[i];
			rri.mapRow = ii;
			// System.out.println("Row " + i + " maps to " + ii);
			for (j = 0; j != matrixSize; j++) {
				RowInfo ri = mCircuitRowInfo[j];
				if (ri.type == RowInfo.ROW_CONST) {
					newrs[ii] -= ri.value * mCircuitMatrix[i][j];
				} else {
					newmatx[ii][ri.mapCol] += mCircuitMatrix[i][j];
				}
			}
			ii++;
		}

		// console("old size = " + matrixSize + " new size = " + newsize);

		mCircuitMatrix = newmatx;
		mCircuitRightSide = newrs;
		matrixSize = mCircuitMatrixSize = newsize;
		for (i = 0; i != matrixSize; i++) {
			mOrigRightSide[i] = mCircuitRightSide[i];
		}
		for (i = 0; i != matrixSize; i++) {
			for (j = 0; j != matrixSize; j++) {
				mOrigMatrix[i][j] = mCircuitMatrix[i][j];
			}
		}
		mCircuitNeedsMap = true;
		return true;
	}

	private CircuitNode getCircuitNode(int n) {
		if (n >= mNodeList.size()) {
			return null;
		}
		return mNodeList.elementAt(n);
	}

	// we need to calculate wire currents for every iteration if someone is viewing
	// a wire in the
	// scope. Otherwise we can do it only once per frame.
	private boolean canDelayWireProcessing() {
		int i;
		for (i = 0; i != mScopeCount; i++) {
			if (mScopes[i].viewingWire()) {
				return false;
			}
		}
		for (i = 0; i != mCirSim.mElmList.size(); i++) {
			if (mCirSim.getElm(i) instanceof ScopeElm && ((ScopeElm) mCirSim.getElm(i)).elmScope.viewingWire()) {
				return false;
			}
		}
		return true;
	}

	// we removed wires from the matrix to speed things up. in order to display wire
	// currents,
	// we need to calculate them now.
	private void calcWireCurrents() {
		int i;

		// for debugging
		// for (i = 0; i != wireInfoList.size(); i++)
		// wireInfoList.get(i).wire.setCurrent(-1, 1.23);

		for (i = 0; i != mWireInfoList.size(); i++) {
			WireInfo wi = mWireInfoList.get(i);
			double cur = 0;
			int j;
			Point p = wi.wire.getPost(wi.post);
			for (j = 0; j != wi.neighbors.size(); j++) {
				CircuitElm ce = wi.neighbors.get(j);
				int n = ce.getNodeAtPoint(p.x, p.y);
				cur += ce.getCurrentIntoNode(n);
			}
			if (wi.post == 0) {
				wi.wire.setCurrent(-1, cur);
			} else {
				wi.wire.setCurrent(-1, -cur);
			}
		}
	}

	// factors a matrix into upper and lower triangular matrices by
	// gaussian elimination. On entry, a[0..n-1][0..n-1] is the
	// matrix to be factored. ipvt[] returns an integer vector of pivot
	// indices, used in the lu_solve() routine.
	private static boolean lu_factor(double a[][], int n, int ipvt[]) {
		int i, j, k;
		// check for a possible singular matrix by scanning for rows that
		// are all zeroes
		for (i = 0; i != n; i++) {
			boolean row_all_zeros = true;
			for (j = 0; j != n; j++) {
				if (a[i][j] != 0) {
					row_all_zeros = false;
					break;
				}
			}
			// if all zeros, it's a singular matrix
			if (row_all_zeros) return false;
		}

		// use Crout's method; loop through the columns
		for (j = 0; j != n; j++) {
			// calculate upper triangular elements for this column
			for (i = 0; i != j; i++) {
				double q = a[i][j];
				for (k = 0; k != i; k++)
					q -= a[i][k] * a[k][j];
				a[i][j] = q;
			}
			// calculate lower triangular elements for this column
			double largest = 0;
			int largestRow = -1;
			for (i = j; i != n; i++) {
				double q = a[i][j];
				for (k = 0; k != j; k++) {
					q -= a[i][k] * a[k][j];
				}
				a[i][j] = q;
				double x = Math.abs(q);
				if (x >= largest) {
					largest = x;
					largestRow = i;
				}
			}
			// pivoting
			if (j != largestRow) {
				double x;
				for (k = 0; k != n; k++) {
					x = a[largestRow][k];
					a[largestRow][k] = a[j][k];
					a[j][k] = x;
				}
			}
			// keep track of row interchanges
			ipvt[j] = largestRow;
			// avoid zeros
			if (a[j][j] == 0.0) {
				System.out.println("avoided zero");
				a[j][j] = 1e-18;
			}
			if (j != n - 1) {
				double mult = 1.0 / a[j][j];
				for (i = j + 1; i != n; i++) {
					a[i][j] *= mult;
				}
			}
		}
		return true;
	}

	// Solves the set of n linear equations using a LU factorization
	// previously performed by lu_factor. On input, b[0..n-1] is the right
	// hand side of the equations, and on output, contains the solution.
	private static void lu_solve(double a[][], int n, int ipvt[], double b[]) {
		int idx;
		// find first nonzero b element
		for (idx = 0; idx != n; idx++) {
			int row = ipvt[idx];
			double swap = b[row];
			b[row] = b[idx];
			b[idx] = swap;
			if (swap != 0) break;
		}

		int bi = idx++;
		for (; idx < n; idx++) {
			int row = ipvt[idx];
			double tot = b[row];
			b[row] = b[idx];
			// forward substitution using the lower triangular matrix
			for (int i = bi; i < idx; i++) {
				tot -= a[idx][i] * b[i];
			}
			b[idx] = tot;
		}
		for (idx = n - 1; idx >= 0; idx--) {
			double tot = b[idx];
			// back-substitution using the upper triangular matrix
			for (int i = idx + 1; i != n; i++) {
				tot -= a[idx][i] * b[i];
			}
			b[idx] = tot / a[idx][idx];
		}
	}

	public static void invertMatrix(double a[][], int n) {
		int ipvt[] = new int[n];
		lu_factor(a, n, ipvt);
		int i, j;
		double b[] = new double[n];
		double inva[][] = new double[n][n];
		// solve for each column of identity matrix
		for (i = 0; i != n; i++) {
			for (j = 0; j != n; j++) {
				b[j] = 0;
			}
			b[i] = 1;
			lu_solve(a, n, ipvt, b);
			for (j = 0; j != n; j++) {
				inva[j][i] = b[j];
			}
		}
		// return in original matrix
		for (i = 0; i != n; i++) {
			for (j = 0; j != n; j++) {
				a[i][j] = inva[i][j];
			}
		}
	}
}
