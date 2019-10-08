/*    
    Copyright (C) Paul Falstad and Iain Sharp
    
    This file is part of CircuitJS1.

    CircuitJS1 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    CircuitJS1 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with CircuitJS1.  If not, see <http://www.gnu.org/licenses/>.
*/

package com.lushprojects.circuitjs1.client;

// GWT conversion (c) 2015 by Iain Sharp

// For information about the theory behind this, see Electronic Circuit & System Simulation Methods by Pillage

import java.util.Vector;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Random;
import java.lang.Math;
import com.google.gwt.canvas.client.Canvas;
import com.google.gwt.user.client.ui.Button;
import com.google.gwt.user.client.ui.CellPanel;
import com.google.gwt.user.client.ui.DialogBox;
import com.google.gwt.user.client.ui.DockLayoutPanel;
import com.google.gwt.user.client.ui.Label;
import com.google.gwt.user.client.ui.RootLayoutPanel;
import com.google.gwt.user.client.ui.RootPanel;
import com.google.gwt.canvas.dom.client.Context2d;
import com.google.gwt.canvas.dom.client.Context2d.LineCap;
import com.google.gwt.event.dom.client.MouseDownEvent;
import com.google.gwt.event.dom.client.MouseDownHandler;
import com.google.gwt.event.dom.client.MouseEvent;
import com.google.gwt.event.dom.client.MouseMoveEvent;
import com.google.gwt.event.dom.client.MouseMoveHandler;
import com.google.gwt.event.dom.client.MouseUpHandler;
import com.google.gwt.event.dom.client.MouseUpEvent;
import com.google.gwt.event.dom.client.MouseOutEvent;
import com.google.gwt.event.dom.client.MouseOutHandler;
import com.google.gwt.event.dom.client.ContextMenuEvent;
import com.google.gwt.event.dom.client.ContextMenuHandler;
import com.google.gwt.user.client.Event.NativePreviewEvent;
import com.google.gwt.user.client.Event.NativePreviewHandler;
import com.google.gwt.event.dom.client.MouseWheelEvent;
import com.google.gwt.event.dom.client.MouseWheelHandler;
import com.google.gwt.core.client.GWT;
import com.google.gwt.core.client.Scheduler;
import com.google.gwt.dom.client.Style.Unit;
import com.google.gwt.http.client.Request;
import com.google.gwt.http.client.RequestException;
import com.google.gwt.http.client.Response;
import com.google.gwt.http.client.RequestBuilder;
import com.google.gwt.http.client.RequestCallback;
import com.google.gwt.user.client.ui.MenuBar;
import com.google.gwt.user.client.Command;
import com.google.gwt.user.client.Event;
import com.google.gwt.user.client.Timer;
import com.google.gwt.user.client.Window;
import com.google.gwt.user.client.ui.VerticalPanel;
import com.google.gwt.user.client.ui.HorizontalPanel;
import com.google.gwt.event.dom.client.ClickHandler;
import com.google.gwt.event.dom.client.ClickEvent;
import com.google.gwt.event.dom.client.DoubleClickHandler;
import com.google.gwt.event.dom.client.DoubleClickEvent;
import com.google.gwt.dom.client.CanvasElement;
import com.google.gwt.dom.client.NativeEvent;
import com.google.gwt.user.client.ui.MenuItem;
import com.google.gwt.safehtml.shared.SafeHtml;
import com.google.gwt.safehtml.shared.SafeHtmlUtils;
import com.google.gwt.storage.client.Storage;
import com.google.gwt.user.client.ui.PopupPanel;
import static com.google.gwt.event.dom.client.KeyCodes.*;
import com.google.gwt.user.client.ui.Frame;
import com.google.gwt.user.client.ui.Widget;
import com.google.gwt.user.client.Window.Navigator;

public class CirSim implements MouseDownHandler, MouseMoveHandler, MouseUpHandler, ClickHandler, DoubleClickHandler,
		ContextMenuHandler, NativePreviewHandler, MouseOutHandler, MouseWheelHandler {

	CircuitManager mCirManager;

	Random random;
	Button resetButton;
	Button runStopButton;
	Button dumpMatrixButton;
	MenuItem aboutItem;
	MenuItem importFromLocalFileItem, importFromTextItem, exportAsUrlItem, exportAsLocalFileItem, exportAsTextItem,
			printItem, recoverItem;
	MenuItem importFromDropboxItem;
	MenuItem undoItem, redoItem, cutItem, copyItem, pasteItem, selectAllItem, optionsItem;
	MenuBar optionsMenuBar;
	CheckboxMenuItem dotsCheckItem;
	CheckboxMenuItem voltsCheckItem;
	CheckboxMenuItem powerCheckItem;
	CheckboxMenuItem smallGridCheckItem;
	CheckboxMenuItem crossHairCheckItem;
	CheckboxMenuItem showValuesCheckItem;
	CheckboxMenuItem conductanceCheckItem;
	CheckboxMenuItem euroResistorCheckItem;
	CheckboxMenuItem euroGatesCheckItem;
	CheckboxMenuItem printableCheckItem;
	CheckboxMenuItem alternativeColorCheckItem;
	CheckboxMenuItem conventionCheckItem;
	private Label powerLabel;
	private Label titleLabel;
	private Scrollbar speedBar;
	private Scrollbar currentBar;
	private Scrollbar powerBar;
	MenuBar elmMenuBar;
	MenuItem elmEditMenuItem;
	MenuItem elmCutMenuItem;
	MenuItem elmCopyMenuItem;
	MenuItem elmDeleteMenuItem;
	MenuItem elmScopeMenuItem;
	MenuItem elmFloatScopeMenuItem;
	MenuItem elmFlipMenuItem;
	MenuItem elmSplitMenuItem;
	MenuItem elmSliderMenuItem;
	MenuBar mainMenuBar;
	MenuItem scopeRemovePlotMenuItem;
	MenuItem scopeSelectYMenuItem;
	ScopePopupMenu scopePopupMenu;
	static HashMap<String, String> localizationMap;

	String lastCursorStyle;
	boolean mouseWasOverSplitter = false;

	// Class addingClass;
	PopupPanel contextPanel = null;
	int mouseMode = MODE_SELECT;
	int tempMouseMode = MODE_SELECT;
	String mouseModeStr = "Select";
	static final double pi = 3.14159265358979323846;
	static final int MODE_ADD_ELM = 0;
	static final int MODE_DRAG_ALL = 1;
	static final int MODE_DRAG_ROW = 2;
	static final int MODE_DRAG_COLUMN = 3;
	static final int MODE_DRAG_SELECTED = 4;
	static final int MODE_DRAG_POST = 5;
	static final int MODE_SELECT = 6;
	static final int MODE_DRAG_SPLITTER = 7;
	static final int infoWidth = 120;
	long mMyframes = 1;
	long mMytime = 0;
	long myruntime = 0;
	long mydrawtime = 0;
	int dragGridX, dragGridY, dragScreenX, dragScreenY, initDragGridX, initDragGridY;
	long mouseDownTime;
	long zoomTime;
	int mouseCursorX = -1;
	int mouseCursorY = -1;
	Rectangle selectedArea;
	int gridSize, gridMask, gridRound;
	boolean dragging;
	// boolean useBufferedImage;
	boolean isMac;
	String ctrlMetaKey;

	int pause = 10;
	int scopeSelected = -1;
	int menuScope = -1;
	int menuPlot = -1;
	int hintType = -1, hintItem1, hintItem2;
	String stopMessage;
	
	static final int HINT_LC = 1;
	static final int HINT_RC = 2;
	static final int HINT_3DB_C = 3;
	static final int HINT_TWINT = 4;
	static final int HINT_3DB_L = 5;
	Vector<CircuitElm> mElmList;
	Vector<Adjustable> adjustables;
	// Vector setupList;
	CircuitElm dragElm, menuElm;
	private CircuitElm mouseElm = null;
	boolean didSwitch = false;
	int mousePost = -1;
	CircuitElm plotXElm, plotYElm;
	int draggingPost;
	SwitchElm heldSwitchElm;

	// public boolean useFrame;

	static EditDialog editDialog, customLogicEditDialog, diodeModelEditDialog;
	static SliderDialog sliderDialog;
	static ImportFromDropbox importFromDropbox;
	static ScrollValuePopup scrollValuePopup;
	static DialogBox dialogShowing;
	static AboutBox aboutBox;
	static ImportFromDropboxDialog importFromDropboxDialog;
	// Class dumpTypes[], shortcuts[];
	String shortcuts[];
	static String muString = "\u03bc";
	static String ohmString = "\u03a9";
	String clipboard;
	String recovery;
	Rectangle circuitArea;
	Vector<String> undoStack, redoStack;
	double transform[];

	DockLayoutPanel layoutPanel;
	MenuBar menuBar;
	MenuBar fileMenuBar;
	VerticalPanel verticalPanel;
	CellPanel buttonPanel;
	private boolean mouseDragging;
	double scopeHeightFraction = 0.2;

	boolean mDcAnalysisFlag;

	Vector<CheckboxMenuItem> mainMenuItems = new Vector<CheckboxMenuItem>();
	Vector<String> mainMenuItemNames = new Vector<String>();

	LoadFile loadFileInput;
	Frame iFrame;

	Canvas cv;
	Context2d cvcontext;
	Canvas backcv;
	Context2d backcontext;
	static final int MENUBARHEIGHT = 30;
	static int VERTICALPANELWIDTH = 166; // default
	static final int POSTGRABSQ = 25;
	static final int MINPOSTGRABSIZE = 256;
	final Timer timer = new Timer() {
		public void run() {
			updateCircuit();
		}
	};
	final int FASTTIMER = 16;

	int getrand(int x) {
		int q = random.nextInt();
		if (q < 0)
			q = -q;
		return q % x;
	}

	public void setCanvasSize() {
		int width, height;
		width = (int) RootLayoutPanel.get().getOffsetWidth();
		height = (int) RootLayoutPanel.get().getOffsetHeight();
		height = height - MENUBARHEIGHT;
		width = width - VERTICALPANELWIDTH;
		if (cv != null) {
			cv.setWidth(width + "PX");
			cv.setHeight(height + "PX");
			cv.setCoordinateSpaceWidth(width);
			cv.setCoordinateSpaceHeight(height);
		}
		if (backcv != null) {
			backcv.setWidth(width + "PX");
			backcv.setHeight(height + "PX");
			backcv.setCoordinateSpaceWidth(width);
			backcv.setCoordinateSpaceHeight(height);
		}

		setCircuitArea();
	}

	void setCircuitArea() {
		int height = cv.getCanvasElement().getHeight();
		int width = cv.getCanvasElement().getWidth();
		int h = (int) ((double) height * scopeHeightFraction);
		/*
		 * if (h < 128 && winSize.height > 300) h = 128;
		 */
		circuitArea = new Rectangle(0, 0, width, height - h);
	}

	// Circuit applet;

	CirSim() {
		// super("Circuit Simulator v1.6d");
		// applet = a;
		// useFrame = false;
		theSim = this;
	}

	String startCircuit = null;
	String startLabel = null;
	String startCircuitText = null;
	String startCircuitLink = null;
	// String baseURL = "http://www.falstad.com/circuit/";

	public void init() {

		boolean printable = false;
		boolean convention = true;
		boolean euroRes = false;
		boolean usRes = false;
		MenuBar m;

		CircuitElm.initClass(this);
		readRecovery();

		QueryParameters qp = new QueryParameters();

		try {
			// baseURL = applet.getDocumentBase().getFile();
			// look for circuit embedded in URL
			// String doc = applet.getDocumentBase().toString();
			String cct = qp.getValue("cct");
			if (cct != null)
				startCircuitText = cct.replace("%24", "$");
			startCircuit = qp.getValue("startCircuit");
			startLabel = qp.getValue("startLabel");
			startCircuitLink = qp.getValue("startCircuitLink");
			euroRes = qp.getBooleanValue("euroResistors", false);
			usRes = qp.getBooleanValue("usResistors", false);
			printable = qp.getBooleanValue("whiteBackground", getOptionFromStorage("whiteBackground", false));
			convention = qp.getBooleanValue("conventionalCurrent", getOptionFromStorage("conventionalCurrent", true));
		} catch (Exception e) {
		}

		boolean euroSetting = false;
		if (euroRes)
			euroSetting = true;
		else if (usRes)
			euroSetting = false;
		else
			euroSetting = getOptionFromStorage("euroResistors", !weAreInUS());
		boolean euroGates = getOptionFromStorage("euroGates", weAreInGermany());

		transform = new double[6];
		String os = Navigator.getPlatform();
		isMac = (os.toLowerCase().contains("mac"));
		ctrlMetaKey = (isMac) ? "Cmd" : "Ctrl";

		shortcuts = new String[127];

		mCirManager = new CircuitManager(this);

		layoutPanel = new DockLayoutPanel(Unit.PX);

		fileMenuBar = new MenuBar(true);
		importFromLocalFileItem = new MenuItem(LS("Open File..."), new MyCommand("file", "importfromlocalfile"));
		importFromLocalFileItem.setEnabled(LoadFile.isSupported());
		fileMenuBar.addItem(importFromLocalFileItem);
		importFromTextItem = new MenuItem(LS("Import From Text..."), new MyCommand("file", "importfromtext"));
		fileMenuBar.addItem(importFromTextItem);
		importFromDropboxItem = new MenuItem(LS("Import From Dropbox..."), new MyCommand("file", "importfromdropbox"));
		fileMenuBar.addItem(importFromDropboxItem);
		exportAsLocalFileItem = new MenuItem(LS("Save As..."), new MyCommand("file", "exportaslocalfile"));
		exportAsLocalFileItem.setEnabled(ExportAsLocalFileDialog.downloadIsSupported());
		fileMenuBar.addItem(exportAsLocalFileItem);
		exportAsUrlItem = new MenuItem(LS("Export As Link..."), new MyCommand("file", "exportasurl"));
		fileMenuBar.addItem(exportAsUrlItem);
		exportAsTextItem = new MenuItem(LS("Export As Text..."), new MyCommand("file", "exportastext"));
		fileMenuBar.addItem(exportAsTextItem);
		fileMenuBar.addItem(new MenuItem(LS("Export As Image..."), new MyCommand("file", "exportasimage")));
		fileMenuBar.addItem(new MenuItem(LS("Create Subcircuit..."), new MyCommand("file", "createsubcircuit")));
		fileMenuBar.addItem(new MenuItem(LS("Find DC Operating Point"), new MyCommand("file", "dcanalysis")));
		recoverItem = new MenuItem(LS("Recover Auto-Save"), new MyCommand("file", "recover"));
		recoverItem.setEnabled(recovery != null);
		fileMenuBar.addItem(recoverItem);
		printItem = new MenuItem(LS("Print..."), new MyCommand("file", "print"));
		fileMenuBar.addItem(printItem);
		fileMenuBar.addSeparator();
		aboutItem = new MenuItem(LS("About..."), (Command) null);
		fileMenuBar.addItem(aboutItem);
		aboutItem.setScheduledCommand(new MyCommand("file", "about"));

		int width = (int) RootLayoutPanel.get().getOffsetWidth();
		VERTICALPANELWIDTH = width / 5;
		if (VERTICALPANELWIDTH > 166)
			VERTICALPANELWIDTH = 166;
		if (VERTICALPANELWIDTH < 128)
			VERTICALPANELWIDTH = 128;

		menuBar = new MenuBar();
		menuBar.addItem(LS("File"), fileMenuBar);
		verticalPanel = new VerticalPanel();

		// make buttons side by side if there's room
		buttonPanel = (VERTICALPANELWIDTH == 166) ? new HorizontalPanel() : new VerticalPanel();

		m = new MenuBar(true);
		m.addItem(undoItem = menuItemWithShortcut(LS("Undo"), LS("Ctrl-Z"), new MyCommand("edit", "undo")));
		m.addItem(redoItem = menuItemWithShortcut(LS("Redo"), LS("Ctrl-Y"), new MyCommand("edit", "redo")));
		m.addSeparator();
		m.addItem(cutItem = menuItemWithShortcut(LS("Cut"), LS("Ctrl-X"), new MyCommand("edit", "cut")));
		m.addItem(copyItem = menuItemWithShortcut(LS("Copy"), LS("Ctrl-C"), new MyCommand("edit", "copy")));
		m.addItem(pasteItem = menuItemWithShortcut(LS("Paste"), LS("Ctrl-V"), new MyCommand("edit", "paste")));
		pasteItem.setEnabled(false);

		m.addItem(menuItemWithShortcut(LS("Duplicate"), LS("Ctrl-D"), new MyCommand("edit", "duplicate")));

		m.addSeparator();
		m.addItem(selectAllItem = menuItemWithShortcut(LS("Select All"), LS("Ctrl-A"),
				new MyCommand("edit", "selectAll")));
		m.addSeparator();
		m.addItem(new MenuItem(weAreInUS() ? LS("Center Circuit") : LS("Centre Circuit"),
				new MyCommand("edit", "centrecircuit")));
		m.addItem(menuItemWithShortcut(LS("Zoom 100%"), "0", new MyCommand("edit", "zoom100")));
		m.addItem(menuItemWithShortcut(LS("Zoom In"), "+", new MyCommand("edit", "zoomin")));
		m.addItem(menuItemWithShortcut(LS("Zoom Out"), "-", new MyCommand("edit", "zoomout")));
		menuBar.addItem(LS("Edit"), m);

		MenuBar drawMenuBar = new MenuBar(true);
		drawMenuBar.setAutoOpen(true);

		menuBar.addItem(LS("Draw"), drawMenuBar);

		m = new MenuBar(true);
		m.addItem(new MenuItem(LS("Stack All"), new MyCommand("scopes", "stackAll")));
		m.addItem(new MenuItem(LS("Unstack All"), new MyCommand("scopes", "unstackAll")));
		m.addItem(new MenuItem(LS("Combine All"), new MyCommand("scopes", "combineAll")));
		menuBar.addItem(LS("Scopes"), m);

		optionsMenuBar = m = new MenuBar(true);
		menuBar.addItem(LS("Options"), optionsMenuBar);
		m.addItem(dotsCheckItem = new CheckboxMenuItem(LS("Show Current")));
		dotsCheckItem.setState(true);
		m.addItem(voltsCheckItem = new CheckboxMenuItem(LS("Show Voltage"), new Command() {
			public void execute() {
				if (voltsCheckItem.getState())
					powerCheckItem.setState(false);
				setPowerBarEnable();
			}
		}));
		voltsCheckItem.setState(true);
		m.addItem(powerCheckItem = new CheckboxMenuItem(LS("Show Power"), new Command() {
			public void execute() {
				if (powerCheckItem.getState())
					voltsCheckItem.setState(false);
				setPowerBarEnable();
			}
		}));
		m.addItem(showValuesCheckItem = new CheckboxMenuItem(LS("Show Values")));
		showValuesCheckItem.setState(true);
		// m.add(conductanceCheckItem = getCheckItem(LS("Show Conductance")));
		m.addItem(smallGridCheckItem = new CheckboxMenuItem(LS("Small Grid"), new Command() {
			public void execute() {
				setGrid();
			}
		}));
		m.addItem(crossHairCheckItem = new CheckboxMenuItem(LS("Show Cursor Cross Hairs"), new Command() {
			public void execute() {
				setOptionInStorage("crossHair", crossHairCheckItem.getState());
			}
		}));
		crossHairCheckItem.setState(getOptionFromStorage("crossHair", false));
		m.addItem(euroResistorCheckItem = new CheckboxMenuItem(LS("European Resistors"), new Command() {
			public void execute() {
				setOptionInStorage("euroResistors", euroResistorCheckItem.getState());
			}
		}));
		euroResistorCheckItem.setState(euroSetting);
		m.addItem(euroGatesCheckItem = new CheckboxMenuItem(LS("IEC Gates"), new Command() {
			public void execute() {
				setOptionInStorage("euroGates", euroGatesCheckItem.getState());
				int i;
				for (i = 0; i != mElmList.size(); i++)
					getElm(i).setPoints();
			}
		}));
		euroGatesCheckItem.setState(euroGates);
		m.addItem(printableCheckItem = new CheckboxMenuItem(LS("White Background"), new Command() {
			public void execute() {
				int i;
				for (i = 0; i < mCirManager.mScopeCount; i++)
					mCirManager.mScopes[i].setRect(mCirManager.mScopes[i].rect);
				setOptionInStorage("whiteBackground", printableCheckItem.getState());
			}
		}));
		printableCheckItem.setState(printable);
		m.addItem(alternativeColorCheckItem = new CheckboxMenuItem(LS("Alt Color for Volts & Pwr"), new Command() {
			public void execute() {

				setOptionInStorage("alternativeColor", alternativeColorCheckItem.getState());
				CircuitElm.setColorScale();
			}
		}));
		alternativeColorCheckItem.setState(getOptionFromStorage("alternativeColor", false));

		m.addItem(conventionCheckItem = new CheckboxMenuItem(LS("Conventional Current Motion"), new Command() {
			public void execute() {
				setOptionInStorage("conventionalCurrent", conventionCheckItem.getState());
			}
		}));
		conventionCheckItem.setState(convention);

		m.addItem(new CheckboxAlignedMenuItem(LS("Shortcuts..."), new MyCommand("options", "shortcuts")));
		m.addItem(optionsItem = new CheckboxAlignedMenuItem(LS("Other Options..."), new MyCommand("options", "other")));

		mainMenuBar = new MenuBar(true);
		mainMenuBar.setAutoOpen(true);
		composeMainMenu(mainMenuBar);
		composeMainMenu(drawMenuBar);
		loadShortcuts();

		layoutPanel.addNorth(menuBar, MENUBARHEIGHT);
		layoutPanel.addEast(verticalPanel, VERTICALPANELWIDTH);
		RootLayoutPanel.get().add(layoutPanel);

		cv = Canvas.createIfSupported();
		if (cv == null) {
			RootPanel.get().add(new Label("Not working. You need a browser that supports the CANVAS element."));
			return;
		}

		cvcontext = cv.getContext2d();
		backcv = Canvas.createIfSupported();
		backcontext = backcv.getContext2d();
		setCanvasSize();
		layoutPanel.add(cv);
		verticalPanel.add(buttonPanel);
		buttonPanel.add(resetButton = new Button(LS("Reset")));
		resetButton.addClickHandler(new ClickHandler() {
			public void onClick(ClickEvent event) {
				mCirManager.resetAction();
			}
		});
		resetButton.setStylePrimaryName("topButton");
		buttonPanel.add(runStopButton = new Button(LSHTML("<Strong>RUN</Strong>&nbsp;/&nbsp;Stop")));
		runStopButton.addClickHandler(new ClickHandler() {
			public void onClick(ClickEvent event) {
				setSimRunning(!mCirManager.simIsRunning());
			}
		});

		/*
		 * dumpMatrixButton = new Button("Dump Matrix");
		 * dumpMatrixButton.addClickHandler(new ClickHandler() { public void
		 * onClick(ClickEvent event) { dumpMatrix = true; }});
		 * verticalPanel.add(dumpMatrixButton);// IES for debugging
		 */

		if (LoadFile.isSupported())
			verticalPanel.add(loadFileInput = new LoadFile(this));

		Label l;
		verticalPanel.add(l = new Label(LS("Simulation Speed")));
		l.addStyleName("topSpace");

		// was max of 140
		verticalPanel.add(speedBar = new Scrollbar(Scrollbar.HORIZONTAL, 3, 1, 0, 260));

		verticalPanel.add(l = new Label(LS("Current Speed")));
		l.addStyleName("topSpace");
		currentBar = new Scrollbar(Scrollbar.HORIZONTAL, 50, 1, 1, 100);
		verticalPanel.add(currentBar);
		verticalPanel.add(powerLabel = new Label(LS("Power Brightness")));
		powerLabel.addStyleName("topSpace");
		verticalPanel.add(powerBar = new Scrollbar(Scrollbar.HORIZONTAL, 50, 1, 1, 100));
		setPowerBarEnable();

		// verticalPanel.add(new Label(""));
		// Font f = new Font("SansSerif", 0, 10);
		l = new Label(LS("Current Circuit:"));
		l.addStyleName("topSpace");
		// l.setFont(f);
		titleLabel = new Label("Label");
		// titleLabel.setFont(f);
		verticalPanel.add(l);
		verticalPanel.add(titleLabel);

		verticalPanel.add(iFrame = new Frame("iframe.html"));
		iFrame.setWidth(VERTICALPANELWIDTH + "px");
		iFrame.setHeight("100 px");
		iFrame.getElement().setAttribute("scrolling", "no");

		setGrid();
		mElmList = new Vector<CircuitElm>();
		adjustables = new Vector<Adjustable>();
		// setupList = new Vector();
		undoStack = new Vector<String>();
		redoStack = new Vector<String>();

		random = new Random();
		// cv.setBackground(Color.black);
		// cv.setForeground(Color.lightGray);

		elmMenuBar = new MenuBar(true);
		elmMenuBar.addItem(elmEditMenuItem = new MenuItem(LS("Edit..."), new MyCommand("elm", "edit")));
		elmMenuBar.addItem(elmScopeMenuItem = new MenuItem(LS("View in Scope"), new MyCommand("elm", "viewInScope")));
		elmMenuBar.addItem(elmFloatScopeMenuItem = new MenuItem(LS("View in Undocked Scope"),
				new MyCommand("elm", "viewInFloatScope")));
		elmMenuBar.addItem(elmCutMenuItem = new MenuItem(LS("Cut"), new MyCommand("elm", "cut")));
		elmMenuBar.addItem(elmCopyMenuItem = new MenuItem(LS("Copy"), new MyCommand("elm", "copy")));
		elmMenuBar.addItem(elmDeleteMenuItem = new MenuItem(LS("Delete"), new MyCommand("elm", "delete")));
		elmMenuBar.addItem(new MenuItem(LS("Duplicate"), new MyCommand("elm", "duplicate")));
		elmMenuBar.addItem(elmFlipMenuItem = new MenuItem(LS("Swap Terminals"), new MyCommand("elm", "flip")));
		elmMenuBar.addItem(elmSplitMenuItem = menuItemWithShortcut(LS("Split Wire"), LS(ctrlMetaKey + "-click"),
				new MyCommand("elm", "split")));
		elmMenuBar.addItem(elmSliderMenuItem = new MenuItem(LS("Sliders..."), new MyCommand("elm", "sliders")));

		scopePopupMenu = new ScopePopupMenu();

		CircuitElm.setColorScale();

		if (startCircuitText != null) {
			getSetupList(false);
			readSetup(startCircuitText, true);
		} else {
			if (stopMessage == null && startCircuitLink != null) {
				readSetup(new byte[] {}, false, true);
				getSetupList(false);
				ImportFromDropboxDialog.setSim(this);
				ImportFromDropboxDialog.doImportDropboxLink(startCircuitLink, false);
			} else {
				readSetup(new byte[] {}, false, true);
				if (stopMessage == null && startCircuit != null) {
					getSetupList(false);
					readSetupFile(startCircuit, startLabel, true);
				} else
					getSetupList(true);
			}
		}

		enableUndoRedo();
		enablePaste();
		setiFrameHeight();
		cv.addMouseDownHandler(this);
		cv.addMouseMoveHandler(this);
		cv.addMouseOutHandler(this);
		cv.addMouseUpHandler(this);
		cv.addClickHandler(this);
		cv.addDoubleClickHandler(this);
		doTouchHandlers(cv.getCanvasElement());
		cv.addDomHandler(this, ContextMenuEvent.getType());
		menuBar.addDomHandler(new ClickHandler() {
			public void onClick(ClickEvent event) {
				doMainMenuChecks();
			}
		}, ClickEvent.getType());
		Event.addNativePreviewHandler(this);
		cv.addMouseWheelHandler(this);
		setSimRunning(true);
	}

	MenuItem menuItemWithShortcut(String text, String shortcut, MyCommand cmd) {
		final String edithtml = "<div style=\"display:inline-block;width:80px;\">";
		String sn = edithtml + text + "</div>" + shortcut;
		return new MenuItem(SafeHtmlUtils.fromTrustedString(sn), cmd);
	}

	boolean getOptionFromStorage(String key, boolean val) {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return val;
		String s = stor.getItem(key);
		if (s == null)
			return val;
		return s == "true";
	}

	void setOptionInStorage(String key, boolean val) {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		stor.setItem(key, val ? "true" : "false");
	}

	// save shortcuts to local storage
	void saveShortcuts() {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		String str = "1";
		int i;
		// format: version;code1=ClassName;code2=ClassName;etc
		for (i = 0; i != shortcuts.length; i++) {
			String sh = shortcuts[i];
			if (sh == null)
				continue;
			str += ";" + i + "=" + sh;
		}
		stor.setItem("shortcuts", str);
	}

	// load shortcuts from local storage
	void loadShortcuts() {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		String str = stor.getItem("shortcuts");
		if (str == null)
			return;
		String keys[] = str.split(";");

		// clear existing shortcuts
		int i;
		for (i = 0; i != shortcuts.length; i++)
			shortcuts[i] = null;

		// clear shortcuts from menu
		for (i = 0; i != mainMenuItems.size(); i++) {
			CheckboxMenuItem item = mainMenuItems.get(i);
			// stop when we get to drag menu items
			if (item.getShortcut().length() > 1)
				break;
			item.setShortcut("");
		}

		// go through keys (skipping version at start)
		for (i = 1; i < keys.length; i++) {
			String arr[] = keys[i].split("=");
			if (arr.length != 2)
				continue;
			int c = Integer.parseInt(arr[0]);
			String className = arr[1];
			shortcuts[c] = className;

			// find menu item and fix it
			int j;
			for (j = 0; j != mainMenuItems.size(); j++) {
				if (mainMenuItemNames.get(j) == className) {
					CheckboxMenuItem item = mainMenuItems.get(j);
					item.setShortcut(Character.toString((char) c));
					break;
				}
			}
		}
	}

	// install touch handlers
	// don't feel like rewriting this in java. Anyway, java doesn't let us create
	// mouse
	// events and dispatch them.
	native void doTouchHandlers(CanvasElement cv) /*-{
													// Set up touch events for mobile, etc
													var lastTap;
													var tmout;
													var sim = this;
													cv.addEventListener("touchstart", function (e) {
													mousePos = getTouchPos(cv, e);
													var touch = e.touches[0];
													var etype = "mousedown";
													clearTimeout(tmout);
													if (e.timeStamp-lastTap < 300) {
													etype = "dblclick";
													} else {
													tmout = setTimeout(function() {
													sim.@com.lushprojects.circuitjs1.client.CirSim::longPress()();
													}, 500);
													}
													lastTap = e.timeStamp;
													
													var mouseEvent = new MouseEvent(etype, {
													clientX: touch.clientX,
													clientY: touch.clientY
													});
													e.preventDefault();
													cv.dispatchEvent(mouseEvent);
													}, false);
													cv.addEventListener("touchend", function (e) {
													var mouseEvent = new MouseEvent("mouseup", {});
													e.preventDefault();
													clearTimeout(tmout);
													cv.dispatchEvent(mouseEvent);
													}, false);
													cv.addEventListener("touchmove", function (e) {
													var touch = e.touches[0];
													var mouseEvent = new MouseEvent("mousemove", {
													clientX: touch.clientX,
													clientY: touch.clientY
													});
													e.preventDefault();
													clearTimeout(tmout);
													cv.dispatchEvent(mouseEvent);
													}, false);
													
													// Get the position of a touch relative to the canvas
													function getTouchPos(canvasDom, touchEvent) {
													var rect = canvasDom.getBoundingClientRect();
													return {
													x: touchEvent.touches[0].clientX - rect.left,
													y: touchEvent.touches[0].clientY - rect.top
													};
													}
													
													}-*/;

	boolean shown = false;

	public void composeMainMenu(MenuBar mainMenuBar) {
		mainMenuBar.addItem(getClassCheckItem(LS("Add Wire"), "WireElm"));
		mainMenuBar.addItem(getClassCheckItem(LS("Add Resistor"), "ResistorElm"));

		MenuBar passMenuBar = new MenuBar(true);
		passMenuBar.addItem(getClassCheckItem(LS("Add Capacitor"), "CapacitorElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Capacitor (polarized)"), "PolarCapacitorElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Inductor"), "InductorElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Switch"), "SwitchElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Push Switch"), "PushSwitchElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add SPDT Switch"), "Switch2Elm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Potentiometer"), "PotElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Transformer"), "TransformerElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Tapped Transformer"), "TappedTransformerElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Transmission Line"), "TransLineElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Relay"), "RelayElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Memristor"), "MemristorElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Spark Gap"), "SparkGapElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Fuse"), "FuseElm"));
		passMenuBar.addItem(getClassCheckItem(LS("Add Custom Transformer"), "CustomTransformerElm"));
		mainMenuBar.addItem(
				SafeHtmlUtils.fromTrustedString(CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Passive Components")),
				passMenuBar);

		MenuBar inputMenuBar = new MenuBar(true);
		inputMenuBar.addItem(getClassCheckItem(LS("Add Ground"), "GroundElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Voltage Source (2-terminal)"), "DCVoltageElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add A/C Voltage Source (2-terminal)"), "ACVoltageElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Voltage Source (1-terminal)"), "RailElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add A/C Voltage Source (1-terminal)"), "ACRailElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Square Wave Source (1-terminal)"), "SquareRailElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Clock"), "ClockElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add A/C Sweep"), "SweepElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Variable Voltage"), "VarRailElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Antenna"), "AntennaElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add AM Source"), "AMElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add FM Source"), "FMElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Current Source"), "CurrentElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Noise Generator"), "NoiseElm"));
		inputMenuBar.addItem(getClassCheckItem(LS("Add Audio Input"), "AudioInputElm"));

		mainMenuBar.addItem(
				SafeHtmlUtils.fromTrustedString(CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Inputs and Sources")),
				inputMenuBar);

		MenuBar outputMenuBar = new MenuBar(true);
		outputMenuBar.addItem(getClassCheckItem(LS("Add Analog Output"), "OutputElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add LED"), "LEDElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Lamp"), "LampElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Text"), "TextElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Box"), "BoxElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Voltmeter/Scobe Probe"), "ProbeElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Ohmmeter"), "OhmMeterElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Labeled Node"), "LabeledNodeElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Test Point"), "TestPointElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Ammeter"), "AmmeterElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Data Export"), "DataRecorderElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Audio Output"), "AudioOutputElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add LED Array"), "LEDArrayElm"));
		outputMenuBar.addItem(getClassCheckItem(LS("Add Stop Trigger"), "StopTriggerElm"));
		mainMenuBar.addItem(
				SafeHtmlUtils.fromTrustedString(CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Outputs and Labels")),
				outputMenuBar);

		MenuBar activeMenuBar = new MenuBar(true);
		activeMenuBar.addItem(getClassCheckItem(LS("Add Diode"), "DiodeElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Zener Diode"), "ZenerElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Transistor (bipolar, NPN)"), "NTransistorElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Transistor (bipolar, PNP)"), "PTransistorElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add MOSFET (N-Channel)"), "NMosfetElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add MOSFET (P-Channel)"), "PMosfetElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add JFET (N-Channel)"), "NJfetElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add JFET (P-Channel)"), "PJfetElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add SCR"), "SCRElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add DIAC"), "DiacElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add TRIAC"), "TriacElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Darlington Pair (NPN)"), "NDarlingtonElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Darlington Pair (PNP)"), "PDarlingtonElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Varactor/Varicap"), "VaractorElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Tunnel Diode"), "TunnelDiodeElm"));
		activeMenuBar.addItem(getClassCheckItem(LS("Add Triode"), "TriodeElm"));
		// activeMenuBar.addItem(getClassCheckItem("Add Photoresistor",
		// "PhotoResistorElm"));
		// activeMenuBar.addItem(getClassCheckItem("Add Thermistor", "ThermistorElm"));
		mainMenuBar.addItem(
				SafeHtmlUtils.fromTrustedString(CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Active Components")),
				activeMenuBar);

		MenuBar activeBlocMenuBar = new MenuBar(true);
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Op Amp (ideal, - on top)"), "OpAmpElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Op Amp (ideal, + on top)"), "OpAmpSwapElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Op Amp (real)"), "OpAmpRealElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Analog Switch (SPST)"), "AnalogSwitchElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Analog Switch (SPDT)"), "AnalogSwitch2Elm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Tristate Buffer"), "TriStateElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Schmitt Trigger"), "SchmittElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Schmitt Trigger (Inverting)"), "InvertingSchmittElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add CCII+"), "CC2Elm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add CCII-"), "CC2NegElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Comparator (Hi-Z/GND output)"), "ComparatorElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add OTA (LM13700 style)"), "OTAElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Voltage-Controlled Voltage Source"), "VCVSElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Voltage-Controlled Current Source"), "VCCSElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Current-Controlled Voltage Source"), "CCVSElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Current-Controlled Current Source"), "CCCSElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Optocoupler"), "OptocouplerElm"));
		activeBlocMenuBar.addItem(getClassCheckItem(LS("Add Subcircuit Instance"), "CustomCompositeElm"));
		mainMenuBar.addItem(SafeHtmlUtils.fromTrustedString(
				CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Active Building Blocks")), activeBlocMenuBar);

		MenuBar gateMenuBar = new MenuBar(true);
		gateMenuBar.addItem(getClassCheckItem(LS("Add Logic Input"), "LogicInputElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add Logic Output"), "LogicOutputElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add Inverter"), "InverterElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add NAND Gate"), "NandGateElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add NOR Gate"), "NorGateElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add AND Gate"), "AndGateElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add OR Gate"), "OrGateElm"));
		gateMenuBar.addItem(getClassCheckItem(LS("Add XOR Gate"), "XorGateElm"));
		mainMenuBar.addItem(SafeHtmlUtils.fromTrustedString(
				CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Logic Gates, Input and Output")), gateMenuBar);

		MenuBar chipMenuBar = new MenuBar(true);
		chipMenuBar.addItem(getClassCheckItem(LS("Add D Flip-Flop"), "DFlipFlopElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add JK Flip-Flop"), "JKFlipFlopElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add T Flip-Flop"), "TFlipFlopElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add 7 Segment LED"), "SevenSegElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add 7 Segment Decoder"), "SevenSegDecoderElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Multiplexer"), "MultiplexerElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Demultiplexer"), "DeMultiplexerElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add SIPO shift register"), "SipoShiftElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add PISO shift register"), "PisoShiftElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Counter"), "CounterElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Ring Counter"), "DecadeElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Latch"), "LatchElm"));
		// chipMenuBar.addItem(getClassCheckItem("Add Static RAM", "SRAMElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Sequence generator"), "SeqGenElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Full Adder"), "FullAdderElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Half Adder"), "HalfAdderElm"));
		chipMenuBar.addItem(getClassCheckItem(LS("Add Custom Logic"), "UserDefinedLogicElm")); // don't change this, it
																								// will break people's
																								// saved shortcuts
		mainMenuBar.addItem(
				SafeHtmlUtils.fromTrustedString(CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Digital Chips")),
				chipMenuBar);

		MenuBar achipMenuBar = new MenuBar(true);
		achipMenuBar.addItem(getClassCheckItem(LS("Add 555 Timer"), "TimerElm"));
		achipMenuBar.addItem(getClassCheckItem(LS("Add Phase Comparator"), "PhaseCompElm"));
		achipMenuBar.addItem(getClassCheckItem(LS("Add DAC"), "DACElm"));
		achipMenuBar.addItem(getClassCheckItem(LS("Add ADC"), "ADCElm"));
		achipMenuBar.addItem(getClassCheckItem(LS("Add VCO"), "VCOElm"));
		achipMenuBar.addItem(getClassCheckItem(LS("Add Monostable"), "MonostableElm"));
		mainMenuBar.addItem(SafeHtmlUtils.fromTrustedString(
				CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Analog and Hybrid Chips")), achipMenuBar);

		MenuBar otherMenuBar = new MenuBar(true);
		CheckboxMenuItem mi;
		otherMenuBar.addItem(mi = getClassCheckItem(LS("Drag All"), "DragAll"));
		mi.setShortcut(LS("(Alt-drag)"));
		otherMenuBar.addItem(mi = getClassCheckItem(LS("Drag Row"), "DragRow"));
		mi.setShortcut(LS("(A-S-drag)"));
		otherMenuBar.addItem(mi = getClassCheckItem(LS("Drag Column"), "DragColumn"));
		mi.setShortcut(isMac ? LS("(A-Cmd-drag)") : LS("(A-M-drag)"));
		otherMenuBar.addItem(getClassCheckItem(LS("Drag Selected"), "DragSelected"));
		otherMenuBar.addItem(mi = getClassCheckItem(LS("Drag Post"), "DragPost"));
		mi.setShortcut("(" + ctrlMetaKey + "-drag)");

		mainMenuBar.addItem(SafeHtmlUtils.fromTrustedString(CheckboxMenuItem.checkBoxHtml + LS("&nbsp;</div>Drag")),
				otherMenuBar);

		mainMenuBar.addItem(mi = getClassCheckItem(LS("Select/Drag Sel"), "Select"));
		mi.setShortcut(LS("(space or Shift-drag)"));
	}

	public void setiFrameHeight() {
		if (iFrame == null)
			return;
		int i;
		int cumheight = 0;
		for (i = 0; i < verticalPanel.getWidgetIndex(iFrame); i++) {
			if (verticalPanel.getWidget(i) != loadFileInput) {
				cumheight = cumheight + verticalPanel.getWidget(i).getOffsetHeight();
				if (verticalPanel.getWidget(i).getStyleName().contains("topSpace"))
					cumheight += 12;
			}
		}
		int ih = RootLayoutPanel.get().getOffsetHeight() - MENUBARHEIGHT - cumheight;
		if (ih < 0)
			ih = 0;
		iFrame.setHeight(ih + "px");
	}

	CheckboxMenuItem getClassCheckItem(String s, String t) {
		// try {
		// Class c = Class.forName(t);
		String shortcut = "";
		CircuitElm elm = constructElement(t, 0, 0);
		CheckboxMenuItem mi;
		// register(c, elm);
		if (elm != null) {
			if (elm.needsShortcut()) {
				shortcut += (char) elm.getShortcut();
				shortcuts[elm.getShortcut()] = t;
			}
			elm.delete();
		}
		// else
		// GWT.log("Coudn't create class: "+t);
		// } catch (Exception ee) {
		// ee.printStackTrace();
		// }
		if (shortcut == "")
			mi = new CheckboxMenuItem(s);
		else
			mi = new CheckboxMenuItem(s, shortcut);
		mi.setScheduledCommand(new MyCommand("main", t));
		mainMenuItems.add(mi);
		mainMenuItemNames.add(t);
		return mi;
	}

	void centreCircuit() {
		Rectangle bounds = getCircuitBounds();

		double scale = 1;

		if (bounds != null)
			// add some space on edges because bounds calculation is not perfect
			scale = Math.min(circuitArea.width / (double) (bounds.width + 140),
					circuitArea.height / (double) (bounds.height + 100));
		scale = Math.min(scale, 1.5); // Limit scale so we don't create enormous circuits in big windows

		// calculate transform so circuit fills most of screen
		transform[0] = transform[3] = scale;
		transform[1] = transform[2] = transform[4] = transform[5] = 0;
		if (bounds != null) {
			transform[4] = (circuitArea.width - bounds.width * scale) / 2 - bounds.x * scale;
			transform[5] = (circuitArea.height - bounds.height * scale) / 2 - bounds.y * scale;
		}
	}

	// get circuit bounds. remember this doesn't use setBbox(). That is calculated
	// when we draw
	// the circuit, but this needs to be ready before we first draw it, so we use
	// this crude method
	Rectangle getCircuitBounds() {
		int i;
		int minx = 1000, maxx = 0, miny = 1000, maxy = 0;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			// centered text causes problems when trying to center the circuit,
			// so we special-case it here
			if (!ce.isCenteredText()) {
				minx = min(ce.x, min(ce.x2, minx));
				maxx = max(ce.x, max(ce.x2, maxx));
			}
			miny = min(ce.y, min(ce.y2, miny));
			maxy = max(ce.y, max(ce.y2, maxy));
		}
		if (minx > maxx)
			return null;
		return new Rectangle(minx, miny, maxx - minx, maxy - miny);
	}

	static CirSim theSim;

	public void setSimRunning(boolean s) {
		if (s) {
			mCirManager.simStart();
			runStopButton.setHTML(LSHTML("<strong>RUN</strong>&nbsp;/&nbsp;Stop"));
			runStopButton.setStylePrimaryName("topButton");
			timer.scheduleRepeating(FASTTIMER);
		} else {
			mCirManager.simStop();
			runStopButton.setHTML(LSHTML("Run&nbsp;/&nbsp;<strong>STOP</strong>"));
			runStopButton.setStylePrimaryName("topButton-red");
			timer.cancel();
			repaint();
		}
	}

	boolean needsRepaint;

	void repaint() {
		if (!needsRepaint) {
			needsRepaint = true;
			Scheduler.get().scheduleFixedDelay(new Scheduler.RepeatingCommand() {
				public boolean execute() {
					updateCircuit();
					needsRepaint = false;
					return false;
				}
			}, FASTTIMER);
		}
	}

	String getHint() {
		CircuitElm c1 = getElm(hintItem1);
		CircuitElm c2 = getElm(hintItem2);
		if (c1 == null || c2 == null)
			return null;
		if (hintType == HINT_LC) {
			if (!(c1 instanceof InductorElm))
				return null;
			if (!(c2 instanceof CapacitorElm))
				return null;
			InductorElm ie = (InductorElm) c1;
			CapacitorElm ce = (CapacitorElm) c2;
			return LS("res.f = ")
					+ CircuitElm.getUnitText(1 / (2 * pi * Math.sqrt(ie.inductance * ce.capacitance)), "Hz");
		}
		if (hintType == HINT_RC) {
			if (!(c1 instanceof ResistorElm))
				return null;
			if (!(c2 instanceof CapacitorElm))
				return null;
			ResistorElm re = (ResistorElm) c1;
			CapacitorElm ce = (CapacitorElm) c2;
			return "RC = " + CircuitElm.getUnitText(re.resistance * ce.capacitance, "s");
		}
		if (hintType == HINT_3DB_C) {
			if (!(c1 instanceof ResistorElm))
				return null;
			if (!(c2 instanceof CapacitorElm))
				return null;
			ResistorElm re = (ResistorElm) c1;
			CapacitorElm ce = (CapacitorElm) c2;
			return LS("f.3db = ") + CircuitElm.getUnitText(1 / (2 * pi * re.resistance * ce.capacitance), "Hz");
		}
		if (hintType == HINT_3DB_L) {
			if (!(c1 instanceof ResistorElm))
				return null;
			if (!(c2 instanceof InductorElm))
				return null;
			ResistorElm re = (ResistorElm) c1;
			InductorElm ie = (InductorElm) c2;
			return LS("f.3db = ") + CircuitElm.getUnitText(re.resistance / (2 * pi * ie.inductance), "Hz");
		}
		if (hintType == HINT_TWINT) {
			if (!(c1 instanceof ResistorElm))
				return null;
			if (!(c2 instanceof CapacitorElm))
				return null;
			ResistorElm re = (ResistorElm) c1;
			CapacitorElm ce = (CapacitorElm) c2;
			return LS("fc = ") + CircuitElm.getUnitText(1 / (2 * pi * re.resistance * ce.capacitance), "Hz");
		}
		return null;
	}

	// public void toggleSwitch(int n) {
	// int i;
	// for (i = 0; i != elmList.size(); i++) {
	// CircuitElm ce = getElm(i);
	// if (ce instanceof SwitchElm) {
	// n--;
	// if (n == 0) {
	// ((SwitchElm) ce).toggle();
	// analyzeFlag = true;
	// cv.repaint();
	// return;
	// }
	// }
	// }
	// }
	void needAnalyze() {
		mCirManager.mAnalyzeFlag = true;
		repaint();
	}

	public CircuitElm getElm(int n) {
		if (n >= mElmList.size()) {
			return null;
		}
		return mElmList.elementAt(n);
	}

	public Adjustable findAdjustable(CircuitElm elm, int item) {
		int i;
		for (i = 0; i != adjustables.size(); i++) {
			Adjustable a = adjustables.get(i);
			if (a.elm == elm && a.editItem == item)
				return a;
		}
		return null;
	}

	public static native void console(String text)
	/*-{
	    console.log(text);
	}-*/;

	public static native void debugger() /*-{ debugger; }-*/;

	double getIterCount() {
		// IES - remove interaction
		if (speedBar.getValue() == 0) return 0;
		return .1 * Math.exp((speedBar.getValue() - 61) / 24.);
	}

	static int min(int a, int b) {
		return (a < b) ? a : b;
	}

	static int max(int a, int b) {
		return (a > b) ? a : b;
	}

	public void menuPerformed(String menu, String item) {
		if (item == "about")
			aboutBox = new AboutBox(circuitjs1.versionString);
		if (item == "importfromlocalfile") {
			pushUndo();
			loadFileInput.click();
		}
		if (item == "importfromtext") {
			dialogShowing = new ImportFromTextDialog(this);
		}
		if (item == "importfromdropbox") {
			importFromDropboxDialog = new ImportFromDropboxDialog(this);
		}
		if (item == "exportasurl") {
			doExportAsUrl();
		}
		if (item == "exportaslocalfile")
			doExportAsLocalFile();
		if (item == "exportastext")
			doExportAsText();
		if (item == "exportasimage")
			doExportAsImage();
		if (item == "createsubcircuit")
			doCreateSubcircuit();
		if (item == "dcanalysis")
			doDCAnalysis();
		if (item == "print")
			doPrint();
		if (item == "recover")
			doRecover();

		if ((menu == "elm" || menu == "scopepop") && contextPanel != null)
			contextPanel.hide();
		if (menu == "options" && item == "shortcuts") {
			dialogShowing = new ShortcutsDialog(this);
			dialogShowing.show();
		}
		if (menu == "options" && item == "other")
			doEdit(new EditOptions(this));
		if (item == "undo")
			doUndo();
		if (item == "redo")
			doRedo();

		// if the mouse is hovering over an element, and a shortcut key is pressed,
		// operate on that element (treat it like a context menu item selection)
		if (menu == "key" && mouseElm != null) {
			menuElm = mouseElm;
			menu = "elm";
		}

		if (item == "cut") {
			if (menu != "elm")
				menuElm = null;
			doCut();
		}
		if (item == "copy") {
			if (menu != "elm")
				menuElm = null;
			doCopy();
		}
		if (item == "paste")
			doPaste(null);
		if (item == "duplicate") {
			if (menu != "elm")
				menuElm = null;
			doDuplicate();
		}
		if (item == "flip")
			doFlip();
		if (item == "split")
			doSplit(menuElm);
		if (item == "selectAll")
			doSelectAll();
		// if (e.getSource() == exitItem) {
		// destroyFrame();
		// return;
		// }

		if (item == "centrecircuit") {
			pushUndo();
			centreCircuit();
		}
		if (item == "stackAll")
			mCirManager.stackAll();
		if (item == "unstackAll")
			mCirManager.unstackAll();
		if (item == "combineAll")
			mCirManager.combineAll();
		if (item == "zoomin")
			zoomCircuit(20);
		if (item == "zoomout")
			zoomCircuit(-20);
		if (item == "zoom100")
			setCircuitScale(1);
		if (menu == "elm" && item == "edit")
			doEdit(menuElm);
		if (item == "delete") {
			if (menu != "elm")
				menuElm = null;
			pushUndo();
			doDelete(true);
		}
		if (item == "sliders")
			doSliders(menuElm);

		if (item == "viewInScope" && menuElm != null) {
			mCirManager.addScope();
		}

		if (item == "viewInFloatScope" && menuElm != null) {
			ScopeElm newScope = new ScopeElm(snapGrid(menuElm.x + 50), snapGrid(menuElm.y + 50));
			mElmList.addElement(newScope);
			newScope.setScopeElm(menuElm);
		}

		if (menu == "scopepop") {
			pushUndo();
			Scope s;
			if (menuScope != -1) {
				s = mCirManager.mScopes[menuScope];
			} else {
				s = ((ScopeElm) mouseElm).elmScope;
			}
			if (item == "dock") {
				if (mCirManager.mScopeCount == mCirManager.mScopes.length) return;
				mCirManager.mScopes[mCirManager.mScopeCount] = ((ScopeElm) mouseElm).elmScope;
				((ScopeElm) mouseElm).clearElmScope();
				mCirManager.mScopes[mCirManager.mScopeCount].position = mCirManager.mScopeCount;
				mCirManager.mScopeCount++;
				doDelete(false);
			}
			if (item == "undock") {
				ScopeElm newScope = new ScopeElm(snapGrid(menuElm.x + 50), snapGrid(menuElm.y + 50));
				mElmList.addElement(newScope);
				newScope.setElmScope(mCirManager.mScopes[menuScope]);

				int i;
				// remove scope from list. setupScopes() will fix the positions
				for (i = menuScope; i < mCirManager.mScopeCount; i++) {
					mCirManager.mScopes[i] = mCirManager.mScopes[i + 1];
				}
				mCirManager.mScopeCount--;
			}
			if (item == "remove")
				s.setElm(null); // setupScopes() will clean this up
			if (item == "removeplot")
				s.removePlot(menuPlot);
			if (item == "speed2")
				s.speedUp();
			if (item == "speed1/2")
				s.slowDown();
			// if (item=="scale")
			// scopes[menuScope].adjustScale(.5);
			if (item == "maxscale")
				s.maxScale();
			if (item == "stack")
				mCirManager.stackScope(menuScope);
			if (item == "unstack")
				mCirManager.unstackScope(menuScope);
			if (item == "combine")
				mCirManager.combineScope(menuScope);
			if (item == "selecty")
				s.selectY();
			if (item == "reset")
				s.resetGraph(true);
			if (item == "properties")
				s.properties();
			deleteUnusedScopeElms();
		}
		if (menu == "circuits" && item.indexOf("setup ") == 0) {
			pushUndo();
			int sp = item.indexOf(' ', 6);
			readSetupFile(item.substring(6, sp), item.substring(sp + 1), true);
		}

		// if (ac.indexOf("setup ") == 0) {
		// pushUndo();
		// readSetupFile(ac.substring(6),
		// ((MenuItem) e.getSource()).getLabel());
		// }

		// IES: Moved from itemStateChanged()
		if (menu == "main") {
			if (contextPanel != null)
				contextPanel.hide();
			// MenuItem mmi = (MenuItem) mi;
			// int prevMouseMode = mouseMode;
			setMouseMode(MODE_ADD_ELM);
			String s = item;
			if (s.length() > 0)
				mouseModeStr = s;
			if (s.compareTo("DragAll") == 0)
				setMouseMode(MODE_DRAG_ALL);
			else if (s.compareTo("DragRow") == 0)
				setMouseMode(MODE_DRAG_ROW);
			else if (s.compareTo("DragColumn") == 0)
				setMouseMode(MODE_DRAG_COLUMN);
			else if (s.compareTo("DragSelected") == 0)
				setMouseMode(MODE_DRAG_SELECTED);
			else if (s.compareTo("DragPost") == 0)
				setMouseMode(MODE_DRAG_POST);
			else if (s.compareTo("Select") == 0)
				setMouseMode(MODE_SELECT);
			// else if (s.length() > 0) {
			// try {
			// addingClass = Class.forName(s);
			// } catch (Exception ee) {
			// ee.printStackTrace();
			// }
			// }
			// else
			// setMouseMode(prevMouseMode);
			tempMouseMode = mouseMode;
		}
		repaint();
	}

	void doEdit(Editable eable) {
		clearSelection();
		pushUndo();
		if (editDialog != null) {
			// requestFocus();
			editDialog.setVisible(false);
			editDialog = null;
		}
		editDialog = new EditDialog(eable, this);
		editDialog.show();
	}

	void doSliders(CircuitElm ce) {
		clearSelection();
		pushUndo();
		if (sliderDialog != null) {
			sliderDialog.setVisible(false);
			sliderDialog = null;
		}
		sliderDialog = new SliderDialog(ce, this);
		sliderDialog.show();
	}

	void doExportAsUrl() {
		String dump = dumpCircuit();
		dialogShowing = new ExportAsUrlDialog(dump);
		dialogShowing.show();
	}

	void doExportAsText() {
		String dump = dumpCircuit();
		dialogShowing = new ExportAsTextDialog(this, dump);
		dialogShowing.show();
	}

	void doExportAsImage() {
		dialogShowing = new ExportAsImageDialog();
		dialogShowing.show();
	}

	void doCreateSubcircuit() {
		EditCompositeModelDialog dlg = new EditCompositeModelDialog();
		if (!dlg.createModel())
			return;
		dlg.createDialog();
		dialogShowing = dlg;
		dialogShowing.show();
	}

	void doExportAsLocalFile() {
		String dump = dumpCircuit();
		dialogShowing = new ExportAsLocalFileDialog(dump);
		dialogShowing.show();
	}

	String dumpCircuit() {
		int i;
		CustomLogicModel.clearDumpedFlags();
		CustomCompositeModel.clearDumpedFlags();
		DiodeModel.clearDumpedFlags();
		int f = (dotsCheckItem.getState()) ? 1 : 0;
		f |= (smallGridCheckItem.getState()) ? 2 : 0;
		f |= (voltsCheckItem.getState()) ? 0 : 4;
		f |= (powerCheckItem.getState()) ? 8 : 0;
		f |= (showValuesCheckItem.getState()) ? 0 : 16;
		// 32 = linear scale in afilter
		String dump = "$ " + f + " " + mCirManager.mDeltaTime + " " + getIterCount() + " " + currentBar.getValue() + " "
				+ CircuitElm.voltageRange + " " + powerBar.getValue() + "\n";

		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			String m = ce.dumpModel();
			if (m != null && !m.isEmpty())
				dump += m + "\n";
			dump += ce.dump() + "\n";
		}
		for (i = 0; i != mCirManager.mScopeCount; i++) {
			String d = mCirManager.mScopes[i].dump();
			if (d != null)
				dump += d + "\n";
		}
		for (i = 0; i != adjustables.size(); i++) {
			Adjustable adj = adjustables.get(i);
			dump += "38 " + adj.dump() + "\n";
		}
		if (hintType != -1)
			dump += "h " + hintType + " " + hintItem1 + " " + hintItem2 + "\n";
		return dump;
	}

	void getSetupList(final boolean openDefault) {

		String url;
		url = GWT.getModuleBaseURL() + "setuplist.txt" + "?v=" + random.nextInt();
		RequestBuilder requestBuilder = new RequestBuilder(RequestBuilder.GET, url);
		try {
			requestBuilder.sendRequest(null, new RequestCallback() {
				public void onError(Request request, Throwable exception) {
					GWT.log("File Error Response", exception);
				}

				public void onResponseReceived(Request request, Response response) {
					// processing goes here
					if (response.getStatusCode() == Response.SC_OK) {
						String text = response.getText();
						processSetupList(text.getBytes(), openDefault);
						// end or processing
					} else
						GWT.log("Bad file server response:" + response.getStatusText());
				}
			});
		} catch (RequestException e) {
			GWT.log("failed file reading", e);
		}
	}

	void processSetupList(byte b[], final boolean openDefault) {
		int len = b.length;
		MenuBar currentMenuBar;
		MenuBar stack[] = new MenuBar[6];
		int stackptr = 0;
		currentMenuBar = new MenuBar(true);
		currentMenuBar.setAutoOpen(true);
		menuBar.addItem(LS("Circuits"), currentMenuBar);
		stack[stackptr++] = currentMenuBar;
		int p;
		for (p = 0; p < len;) {
			int l;
			for (l = 0; l != len - p; l++)
				if (b[l + p] == '\n') {
					l++;
					break;
				}
			String line = new String(b, p, l - 1);
			if (line.charAt(0) == '#')
				;
			else if (line.charAt(0) == '+') {
				// MenuBar n = new Menu(line.substring(1));
				MenuBar n = new MenuBar(true);
				n.setAutoOpen(true);
				currentMenuBar.addItem(LS(line.substring(1)), n);
				currentMenuBar = stack[stackptr++] = n;
			} else if (line.charAt(0) == '-') {
				currentMenuBar = stack[--stackptr - 1];
			} else {
				int i = line.indexOf(' ');
				if (i > 0) {
					String title = LS(line.substring(i + 1));
					boolean first = false;
					if (line.charAt(0) == '>')
						first = true;
					String file = line.substring(first ? 1 : 0, i);
					currentMenuBar
							.addItem(new MenuItem(title, new MyCommand("circuits", "setup " + file + " " + title)));
					if (file.equals(startCircuit) && startLabel == null) {
						startLabel = title;
						titleLabel.setText(title);
					}
					if (first && startCircuit == null) {
						startCircuit = file;
						startLabel = title;
						if (openDefault && stopMessage == null)
							readSetupFile(startCircuit, startLabel, true);
					}
				}
			}
			p += l;
		}
	}

	void readSetup(String text, boolean centre) {
		readSetup(text, false, centre);
		titleLabel.setText(null);
	}

	void readSetup(String text, boolean retain, boolean centre) {
		readSetup(text.getBytes(), retain, centre);
		titleLabel.setText(null);
	}

	void setCircuitTitle(String s) {
		if (s != null)
			titleLabel.setText(s);
	}

	void readSetupFile(String str, String title, boolean centre) {
		mCirManager.mTime = 0;
		System.out.println(str);
		// TODO: Maybe think about some better approach to cache management!
		String url = GWT.getModuleBaseURL() + "circuits/" + str + "?v=" + random.nextInt();
		loadFileFromURL(url, centre);
		if (title != null)
			titleLabel.setText(title);
	}

	void loadFileFromURL(String url, final boolean centre) {
		RequestBuilder requestBuilder = new RequestBuilder(RequestBuilder.GET, url);

		try {
			requestBuilder.sendRequest(null, new RequestCallback() {
				public void onError(Request request, Throwable exception) {
					GWT.log("File Error Response", exception);
				}

				public void onResponseReceived(Request request, Response response) {
					if (response.getStatusCode() == Response.SC_OK) {
						String text = response.getText();
						readSetup(text.getBytes(), false, centre);
					} else
						GWT.log("Bad file server response:" + response.getStatusText());
				}
			});
		} catch (RequestException e) {
			GWT.log("failed file reading", e);
		}

	}

	void readSetup(byte b[], boolean retain, boolean centre) {
		int i;
		int len = b.length;
		if (!retain) {
			clearMouseElm();
			for (i = 0; i != mElmList.size(); i++) {
				CircuitElm ce = getElm(i);
				ce.delete();
			}
			mElmList.removeAllElements();
			hintType = -1;
			mCirManager.mDeltaTime = 5e-6;
			dotsCheckItem.setState(false);
			smallGridCheckItem.setState(false);
			powerCheckItem.setState(false);
			voltsCheckItem.setState(true);
			showValuesCheckItem.setState(true);
			setGrid();
			speedBar.setValue(117); // 57
			currentBar.setValue(50);
			powerBar.setValue(50);
			CircuitElm.voltageRange = 5;
			mCirManager.mScopeCount = 0;
			mCirManager.mLastIterTime = 0;
		}
		// cv.repaint();
		int p;
		for (p = 0; p < len;) {
			int l;
			int linelen = len - p; // IES - changed to allow the last line to not end with a delim.
			for (l = 0; l != len - p; l++)
				if (b[l + p] == '\n' || b[l + p] == '\r') {
					linelen = l++;
					if (l + p < b.length && b[l + p] == '\n')
						l++;
					break;
				}
			String line = new String(b, p, linelen);
			StringTokenizer st = new StringTokenizer(line, " +\t\n\r\f");
			while (st.hasMoreTokens()) {
				String type = st.nextToken();
				int tint = type.charAt(0);
				try {
					if (tint == 'o') {
						Scope sc = new Scope(this);
						sc.position = mCirManager.mScopeCount;
						sc.undump(st);
						mCirManager.mScopes[mCirManager.mScopeCount++] = sc;
						break;
					}
					if (tint == 'h') {
						readHint(st);
						break;
					}
					if (tint == '$') {
						readOptions(st);
						break;
					}
					if (tint == '!') {
						CustomLogicModel.undumpModel(st);
						break;
					}
					if (tint == '%' || tint == '?' || tint == 'B') {
						// ignore afilter-specific stuff
						break;
					}
					// do not add new symbols here without testing export as link

					// if first character is a digit then parse the type as a number
					if (tint >= '0' && tint <= '9')
						tint = new Integer(type).intValue();

					if (tint == 34) {
						DiodeModel.undumpModel(st);
						break;
					}
					if (tint == 38) {
						Adjustable adj = new Adjustable(st, this);
						adjustables.add(adj);
						break;
					}
					if (tint == '.') {
						CustomCompositeModel.undumpModel(st);
						break;
					}
					int x1 = new Integer(st.nextToken()).intValue();
					int y1 = new Integer(st.nextToken()).intValue();
					int x2 = new Integer(st.nextToken()).intValue();
					int y2 = new Integer(st.nextToken()).intValue();
					int f = new Integer(st.nextToken()).intValue();

					CircuitElm newce = createCe(tint, x1, y1, x2, y2, f, st);
					if (newce == null) {
						System.out.println("unrecognized dump type: " + type);
						break;
					}
					newce.setPoints();
					mElmList.addElement(newce);
				} catch (Exception ee) {
					ee.printStackTrace();
					console("exception while undumping " + ee);
					break;
				}
				break;
			}
			p += l;

		}
		setPowerBarEnable();
		enableItems();
		if (!retain) {
			// create sliders as needed
			for (i = 0; i != adjustables.size(); i++)
				adjustables.get(i).createSlider(this);
		}
		// if (!retain)
		// handleResize(); // for scopes
		needAnalyze();
		if (centre)
			centreCircuit();

		AudioInputElm.clearCache(); // to save memory
	}

	// delete sliders for an element
	void deleteSliders(CircuitElm elm) {
		int i;
		if (adjustables == null)
			return;
		for (i = adjustables.size() - 1; i >= 0; i--) {
			Adjustable adj = adjustables.get(i);
			if (adj.elm == elm) {
				adj.deleteSlider(this);
				adjustables.remove(i);
			}
		}
	}

	void readHint(StringTokenizer st) {
		hintType = new Integer(st.nextToken()).intValue();
		hintItem1 = new Integer(st.nextToken()).intValue();
		hintItem2 = new Integer(st.nextToken()).intValue();
	}

	void readOptions(StringTokenizer st) {
		int flags = new Integer(st.nextToken()).intValue();
		dotsCheckItem.setState((flags & 1) != 0);
		smallGridCheckItem.setState((flags & 2) != 0);
		voltsCheckItem.setState((flags & 4) == 0);
		powerCheckItem.setState((flags & 8) == 8);
		showValuesCheckItem.setState((flags & 16) == 0);
		mCirManager.mDeltaTime = new Double(st.nextToken()).doubleValue();
		double sp = new Double(st.nextToken()).doubleValue();
		int sp2 = (int) (Math.log(10 * sp) * 24 + 61.5);
		// int sp2 = (int) (Math.log(sp)*24+1.5);
		speedBar.setValue(sp2);
		currentBar.setValue(new Integer(st.nextToken()).intValue());
		CircuitElm.voltageRange = new Double(st.nextToken()).doubleValue();

		try {
			powerBar.setValue(new Integer(st.nextToken()).intValue());
		} catch (Exception e) {
		}
		setGrid();
	}

	int snapGrid(int x) {
		return (x + gridRound) & gridMask;
	}

	boolean doSwitch(int x, int y) {
		if (mouseElm == null || !(mouseElm instanceof SwitchElm))
			return false;
		SwitchElm se = (SwitchElm) mouseElm;
		if (!se.getSwitchRect().contains(x, y))
			return false;
		se.toggle();
		if (se.momentary)
			heldSwitchElm = se;
		needAnalyze();
		return true;
	}

	int locateElm(CircuitElm elm) {
		int i;
		for (i = 0; i != mElmList.size(); i++)
			if (elm == mElmList.elementAt(i))
				return i;
		return -1;
	}

	public void mouseDragged(MouseMoveEvent e) {
		// ignore right mouse button with no modifiers (needed on PC)
		if (e.getNativeButton() == NativeEvent.BUTTON_RIGHT) {
			if (!(e.isMetaKeyDown() || e.isShiftKeyDown() || e.isControlKeyDown() || e.isAltKeyDown()))
				return;
		}

		if (tempMouseMode == MODE_DRAG_SPLITTER) {
			dragSplitter(e.getX(), e.getY());
			return;
		}
		int gx = inverseTransformX(e.getX());
		int gy = inverseTransformY(e.getY());
		if (!circuitArea.contains(e.getX(), e.getY()))
			return;
		boolean changed = false;
		if (dragElm != null)
			dragElm.drag(gx, gy);
		boolean success = true;
		switch (tempMouseMode) {
		case MODE_DRAG_ALL:
			dragAll(e.getX(), e.getY());
			break;
		case MODE_DRAG_ROW:
			dragRow(snapGrid(gx), snapGrid(gy));
			changed = true;
			break;
		case MODE_DRAG_COLUMN:
			dragColumn(snapGrid(gx), snapGrid(gy));
			changed = true;
			break;
		case MODE_DRAG_POST:
			if (mouseElm != null) {
				dragPost(snapGrid(gx), snapGrid(gy));
				changed = true;
			}
			break;
		case MODE_SELECT:
			if (mouseElm == null)
				selectArea(gx, gy);
			else {
				// wait short delay before dragging. This is to fix problem where switches were
				// accidentally getting
				// dragged when tapped on mobile devices
				if (System.currentTimeMillis() - mouseDownTime < 150)
					return;

				tempMouseMode = MODE_DRAG_SELECTED;
				changed = success = dragSelected(gx, gy);
			}
			break;
		case MODE_DRAG_SELECTED:
			changed = success = dragSelected(gx, gy);
			break;

		}
		dragging = true;
		if (success) {
			dragScreenX = e.getX();
			dragScreenY = e.getY();
			// console("setting dragGridx in mousedragged");
			dragGridX = inverseTransformX(dragScreenX);
			dragGridY = inverseTransformY(dragScreenY);
			if (!(tempMouseMode == MODE_DRAG_SELECTED && onlyGraphicsElmsSelected())) {
				dragGridX = snapGrid(dragGridX);
				dragGridY = snapGrid(dragGridY);
			}
		}
		if (changed)
			writeRecoveryToStorage();
		repaint();
	}

	void dragSplitter(int x, int y) {
		double h = (double) cv.getCanvasElement().getHeight();
		if (h < 1)
			h = 1;
		scopeHeightFraction = 1.0 - (((double) y) / h);
		if (scopeHeightFraction < 0.1)
			scopeHeightFraction = 0.1;
		if (scopeHeightFraction > 0.9)
			scopeHeightFraction = 0.9;
		setCircuitArea();
		repaint();
	}

	void dragAll(int x, int y) {
		int dx = x - dragScreenX;
		int dy = y - dragScreenY;
		if (dx == 0 && dy == 0)
			return;
		transform[4] += dx;
		transform[5] += dy;
		dragScreenX = x;
		dragScreenY = y;
	}

	void dragRow(int x, int y) {
		int dy = y - dragGridY;
		if (dy == 0)
			return;
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			if (ce.y == dragGridY)
				ce.movePoint(0, 0, dy);
			if (ce.y2 == dragGridY)
				ce.movePoint(1, 0, dy);
		}
		removeZeroLengthElements();
	}

	void dragColumn(int x, int y) {
		int dx = x - dragGridX;
		if (dx == 0)
			return;
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			if (ce.x == dragGridX)
				ce.movePoint(0, dx, 0);
			if (ce.x2 == dragGridX)
				ce.movePoint(1, dx, 0);
		}
		removeZeroLengthElements();
	}

	boolean onlyGraphicsElmsSelected() {
		if (mouseElm != null && !(mouseElm instanceof GraphicElm))
			return false;
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			if (ce.isSelected() && !(ce instanceof GraphicElm))
				return false;
		}
		return true;
	}

	boolean dragSelected(int x, int y) {
		boolean me = false;
		int i;
		if (mouseElm != null && !mouseElm.isSelected())
			mouseElm.setSelected(me = true);

		if (!onlyGraphicsElmsSelected()) {
			// console("Snapping x and y");
			x = snapGrid(x);
			y = snapGrid(y);
		}

		int dx = x - dragGridX;
		// console("dx="+dx+"dragGridx="+dragGridX);
		int dy = y - dragGridY;
		if (dx == 0 && dy == 0) {
			// don't leave mouseElm selected if we selected it above
			if (me)
				mouseElm.setSelected(false);
			return false;
		}
		boolean allowed = true;

		// check if moves are allowed
		for (i = 0; allowed && i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			if (ce.isSelected() && !ce.allowMove(dx, dy))
				allowed = false;
		}

		if (allowed) {
			for (i = 0; i != mElmList.size(); i++) {
				CircuitElm ce = getElm(i);
				if (ce.isSelected())
					ce.move(dx, dy);
			}
			needAnalyze();
		}

		// don't leave mouseElm selected if we selected it above
		if (me)
			mouseElm.setSelected(false);

		return allowed;
	}

	void dragPost(int x, int y) {
		if (draggingPost == -1) {
			draggingPost = (Graphics.distanceSq(mouseElm.x, mouseElm.y, x, y) > Graphics.distanceSq(mouseElm.x2,
					mouseElm.y2, x, y)) ? 1 : 0;
		}
		int dx = x - dragGridX;
		int dy = y - dragGridY;
		if (dx == 0 && dy == 0)
			return;
		mouseElm.movePoint(draggingPost, dx, dy);
		needAnalyze();
	}

	void doFlip() {
		menuElm.flipPosts();
		needAnalyze();
	}

	void doSplit(CircuitElm ce) {
		int x = snapGrid(inverseTransformX(menuX));
		int y = snapGrid(inverseTransformY(menuY));
		if (ce == null || !(ce instanceof WireElm))
			return;
		if (ce.x == ce.x2)
			x = ce.x;
		else
			y = ce.y;

		// don't create zero-length wire
		if (x == ce.x && y == ce.y || x == ce.x2 && y == ce.y2)
			return;

		WireElm newWire = new WireElm(x, y);
		newWire.drag(ce.x2, ce.y2);
		ce.drag(x, y);
		mElmList.addElement(newWire);
		needAnalyze();
	}

	void selectArea(int x, int y) {
		int x1 = min(x, initDragGridX);
		int x2 = max(x, initDragGridX);
		int y1 = min(y, initDragGridY);
		int y2 = max(y, initDragGridY);
		selectedArea = new Rectangle(x1, y1, x2 - x1, y2 - y1);
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			ce.selectRect(selectedArea);
		}
	}

	// void setSelectedElm(CircuitElm cs) {
	// int i;
	// for (i = 0; i != elmList.size(); i++) {
	// CircuitElm ce = getElm(i);
	// ce.setSelected(ce == cs);
	// }
	// mouseElm = cs;
	// }

	void setMouseElm(CircuitElm ce) {
		if (ce != mouseElm) {
			if (mouseElm != null)
				mouseElm.setMouseElm(false);
			if (ce != null)
				ce.setMouseElm(true);
			mouseElm = ce;
		}
	}

	void removeZeroLengthElements() {
		int i;
		for (i = mElmList.size() - 1; i >= 0; i--) {
			CircuitElm ce = getElm(i);
			if (ce.x == ce.x2 && ce.y == ce.y2) {
				mElmList.removeElementAt(i);
				ce.delete();
			}
		}
		needAnalyze();
	}

	boolean mouseIsOverSplitter(int x, int y) {
		boolean isOverSplitter;
		isOverSplitter = ((x >= 0) && (x < circuitArea.width) && (y >= circuitArea.height - 5)
				&& (y < circuitArea.height));
		if (isOverSplitter != mouseWasOverSplitter) {
			if (isOverSplitter)
				setCursorStyle("cursorSplitter");
			else
				setMouseMode(mouseMode);
		}
		mouseWasOverSplitter = isOverSplitter;
		return isOverSplitter;
	}

	public void onMouseMove(MouseMoveEvent e) {
		e.preventDefault();
		mouseCursorX = e.getX();
		mouseCursorY = e.getY();
		if (mouseDragging) {
			mouseDragged(e);
			return;
		}
		mouseSelect(e);
	}

	// convert screen coordinates to grid coordinates by inverting circuit transform
	int inverseTransformX(double x) {
		return (int) ((x - transform[4]) / transform[0]);
	}

	int inverseTransformY(double y) {
		return (int) ((y - transform[5]) / transform[3]);
	}

	// convert grid coordinates to screen coordinates
	int transformX(double x) {
		return (int) ((x * transform[0]) + transform[4]);
	}

	int transformY(double y) {
		return (int) ((y * transform[3]) + transform[5]);
	}

	// need to break this out into a separate routine to handle selection,
	// since we don't get mouse move events on mobile
	public void mouseSelect(MouseEvent<?> e) {
		// The following is in the original, but seems not to work/be needed for GWT
		// if (e.getNativeButton()==NativeEvent.BUTTON_LEFT)
		// return;
		CircuitElm newMouseElm = null;
		mouseCursorX = e.getX();
		mouseCursorY = e.getY();
		int sx = e.getX();
		int sy = e.getY();
		int gx = inverseTransformX(sx);
		int gy = inverseTransformY(sy);
		// console("Settingd draggridx in mouseEvent");
		dragGridX = snapGrid(gx);
		dragGridY = snapGrid(gy);
		dragScreenX = sx;
		dragScreenY = sy;
		draggingPost = -1;
		int i;
		// CircuitElm origMouse = mouseElm;

		mousePost = -1;
		plotXElm = plotYElm = null;

		if (mouseIsOverSplitter(sx, sy)) {
			setMouseElm(null);
			return;
		}

		if (mouseElm != null && (mouseElm.getHandleGrabbedClose(gx, gy, POSTGRABSQ, MINPOSTGRABSIZE) >= 0)) {
			newMouseElm = mouseElm;
		} else {
			int bestDist = 100000000;
			int bestArea = 100000000;
			for (i = 0; i != mElmList.size(); i++) {
				CircuitElm ce = getElm(i);
				if (ce.boundingBox.contains(gx, gy)) {
					int j;
					int area = ce.boundingBox.width * ce.boundingBox.height;
					int jn = ce.getPostCount();
					if (jn > 2)
						jn = 2;
					for (j = 0; j != jn; j++) {
						Point pt = ce.getPost(j);
						int dist = Graphics.distanceSq(gx, gy, pt.x, pt.y);

						// if multiple elements have overlapping bounding boxes,
						// we prefer selecting elements that have posts close
						// to the mouse pointer and that have a small bounding
						// box area.
						if (dist <= bestDist && area <= bestArea) {
							bestDist = dist;
							bestArea = area;
							newMouseElm = ce;
						}
					}
					// prefer selecting elements that have small bounding box area (for
					// elements with no posts)
					if (ce.getPostCount() == 0 && area <= bestArea) {
						newMouseElm = ce;
						bestArea = area;
					}
				}
			} // for
		}
		scopeSelected = -1;
		if (newMouseElm == null) {
			for (i = 0; i != mCirManager.mScopeCount; i++) {
				Scope s = mCirManager.mScopes[i];
				if (s.rect.contains(sx, sy)) {
					newMouseElm = s.getElm();
					if (s.plotXY) {
						plotXElm = s.getXElm();
						plotYElm = s.getYElm();
					}
					scopeSelected = i;
				}
			}
			// // the mouse pointer was not in any of the bounding boxes, but we
			// // might still be close to a post
			for (i = 0; i != mElmList.size(); i++) {
				CircuitElm ce = getElm(i);
				if (mouseMode == MODE_DRAG_POST) {
					if (ce.getHandleGrabbedClose(gx, gy, POSTGRABSQ, 0) > 0) {
						newMouseElm = ce;
						break;
					}
				}
				int j;
				int jn = ce.getPostCount();
				for (j = 0; j != jn; j++) {
					Point pt = ce.getPost(j);
					// int dist = Graphics.distanceSq(x, y, pt.x, pt.y);
					if (Graphics.distanceSq(pt.x, pt.y, gx, gy) < 26) {
						newMouseElm = ce;
						mousePost = j;
						break;
					}
				}
			}
		} else {
			mousePost = -1;
			// look for post close to the mouse pointer
			for (i = 0; i != newMouseElm.getPostCount(); i++) {
				Point pt = newMouseElm.getPost(i);
				if (Graphics.distanceSq(pt.x, pt.y, gx, gy) < 26)
					mousePost = i;
			}
		}
		repaint();
		setMouseElm(newMouseElm);
	}

	public void onContextMenu(ContextMenuEvent e) {
		e.preventDefault();
		menuClientX = e.getNativeEvent().getClientX();
		menuClientY = e.getNativeEvent().getClientY();
		doPopupMenu();
	}

	void doPopupMenu() {
		menuElm = mouseElm;
		menuScope = -1;
		menuPlot = -1;
		int x, y;
		if (scopeSelected != -1) {
			if (mCirManager.mScopes[scopeSelected].canMenu()) {
				menuScope = scopeSelected;
				menuPlot = mCirManager.mScopes[scopeSelected].selectedPlot;
				scopePopupMenu.doScopePopupChecks(false, mCirManager.mScopes[scopeSelected]);
				contextPanel = new PopupPanel(true);
				contextPanel.add(scopePopupMenu.getMenuBar());
				y = Math.max(0, Math.min(menuClientY, cv.getCoordinateSpaceHeight() - 160));
				contextPanel.setPopupPosition(menuClientX, y);
				contextPanel.show();
			}
		} else if (mouseElm != null) {
			if (!(mouseElm instanceof ScopeElm)) {
				elmScopeMenuItem.setEnabled(mouseElm.canViewInScope());
				elmFloatScopeMenuItem.setEnabled(mouseElm.canViewInScope());
				elmEditMenuItem.setEnabled(mouseElm.getEditInfo(0) != null);
				elmFlipMenuItem.setEnabled(mouseElm.getPostCount() == 2);
				elmSplitMenuItem.setEnabled(canSplit(mouseElm));
				elmSliderMenuItem.setEnabled(sliderItemEnabled(mouseElm));
				contextPanel = new PopupPanel(true);
				contextPanel.add(elmMenuBar);
				contextPanel.setPopupPosition(menuClientX, menuClientY);
				contextPanel.show();
			} else {
				ScopeElm s = (ScopeElm) mouseElm;
				if (s.elmScope.canMenu()) {
					menuPlot = s.elmScope.selectedPlot;
					scopePopupMenu.doScopePopupChecks(true, s.elmScope);
					contextPanel = new PopupPanel(true);
					contextPanel.add(scopePopupMenu.getMenuBar());
					contextPanel.setPopupPosition(menuClientX, menuClientY);
					contextPanel.show();
				}
			}
		} else {
			doMainMenuChecks();
			contextPanel = new PopupPanel(true);
			contextPanel.add(mainMenuBar);
			x = Math.max(0, Math.min(menuClientX, cv.getCoordinateSpaceWidth() - 400));
			y = Math.max(0, Math.min(menuClientY, cv.getCoordinateSpaceHeight() - 450));
			contextPanel.setPopupPosition(x, y);
			contextPanel.show();
		}
	}

	boolean canSplit(CircuitElm ce) {
		if (!(ce instanceof WireElm))
			return false;
		WireElm we = (WireElm) ce;
		if (we.x == we.x2 || we.y == we.y2)
			return true;
		return false;
	}

	// check if the user can create sliders for this element
	boolean sliderItemEnabled(CircuitElm elm) {
		int i;

		// prevent confusion
		if (elm instanceof VarRailElm || elm instanceof PotElm)
			return false;

		for (i = 0;; i++) {
			EditInfo ei = elm.getEditInfo(i);
			if (ei == null)
				return false;
			if (ei.canCreateAdjustable())
				return true;
		}
	}

	void longPress() {
		doPopupMenu();
	}

	// public void mouseClicked(MouseEvent e) {
	public void onClick(ClickEvent e) {
		e.preventDefault();
		// //IES - remove inteaction
		//// if ( e.getClickCount() == 2 && !didSwitch )
		//// doEditMenu(e);
		// if (e.getNativeButton() == NativeEvent.BUTTON_LEFT) {
		// if (mouseMode == MODE_SELECT || mouseMode == MODE_DRAG_SELECTED)
		// clearSelection();
		// }
		if ((e.getNativeButton() == NativeEvent.BUTTON_MIDDLE))
			scrollValues(e.getNativeEvent().getClientX(), e.getNativeEvent().getClientY(), 0);
	}

	public void onDoubleClick(DoubleClickEvent e) {
		e.preventDefault();
		// if (!didSwitch && mouseElm != null)
		if (mouseElm != null && !(mouseElm instanceof SwitchElm))
			doEdit(mouseElm);
	}

	// public void mouseEntered(MouseEvent e) {
	// }

	public void onMouseOut(MouseOutEvent e) {
		mouseCursorX = -1;
	}

	void clearMouseElm() {
		scopeSelected = -1;
		setMouseElm(null);
		plotXElm = plotYElm = null;
	}

	int menuClientX, menuClientY;
	int menuX, menuY;

	public void onMouseDown(MouseDownEvent e) {
		// public void mousePressed(MouseEvent e) {
		e.preventDefault();
		menuX = menuClientX = e.getX();
		menuY = menuClientY = e.getY();
		mouseDownTime = System.currentTimeMillis();

		// maybe someone did copy in another window? should really do this when
		// window receives focus
		enablePaste();

		if (e.getNativeButton() != NativeEvent.BUTTON_LEFT && e.getNativeButton() != NativeEvent.BUTTON_MIDDLE)
			return;

		// set mouseElm in case we are on mobile
		mouseSelect(e);

		mouseDragging = true;
		didSwitch = false;

		if (mouseWasOverSplitter) {
			tempMouseMode = MODE_DRAG_SPLITTER;
			return;
		}
		if (e.getNativeButton() == NativeEvent.BUTTON_LEFT) {
			// // left mouse
			tempMouseMode = mouseMode;
			if (e.isAltKeyDown() && e.isMetaKeyDown())
				tempMouseMode = MODE_DRAG_COLUMN;
			else if (e.isAltKeyDown() && e.isShiftKeyDown())
				tempMouseMode = MODE_DRAG_ROW;
			else if (e.isShiftKeyDown())
				tempMouseMode = MODE_SELECT;
			else if (e.isAltKeyDown())
				tempMouseMode = MODE_DRAG_ALL;
			else if (e.isControlKeyDown() || e.isMetaKeyDown())
				tempMouseMode = MODE_DRAG_POST;
		} else
			tempMouseMode = MODE_DRAG_ALL;

		if ((scopeSelected != -1 && mCirManager.mScopes[scopeSelected].cursorInSettingsWheel()) || (scopeSelected == -1
				&& mouseElm instanceof ScopeElm && ((ScopeElm) mouseElm).elmScope.cursorInSettingsWheel())) {
			console("Doing something");
			Scope s;
			if (scopeSelected != -1)
				s = mCirManager.mScopes[scopeSelected];
			else
				s = ((ScopeElm) mouseElm).elmScope;
			s.properties();
			clearSelection();
			mouseDragging = false;
			return;
		}

		int gx = inverseTransformX(e.getX());
		int gy = inverseTransformY(e.getY());
		if (doSwitch(gx, gy)) {
			// do this BEFORE we change the mouse mode to MODE_DRAG_POST! Or else logic
			// inputs
			// will add dots to the whole circuit when we click on them!
			didSwitch = true;
			return;
		}

		// IES - Grab resize handles in select mode if they are far enough apart and you
		// are on top of them
		if (tempMouseMode == MODE_SELECT && mouseElm != null
				&& mouseElm.getHandleGrabbedClose(gx, gy, POSTGRABSQ, MINPOSTGRABSIZE) >= 0 && !anySelectedButMouse())
			tempMouseMode = MODE_DRAG_POST;

		if (tempMouseMode != MODE_SELECT && tempMouseMode != MODE_DRAG_SELECTED)
			clearSelection();

		pushUndo();
		initDragGridX = gx;
		initDragGridY = gy;
		dragging = true;
		if (tempMouseMode != MODE_ADD_ELM)
			return;
		//
		int x0 = snapGrid(gx);
		int y0 = snapGrid(gy);
		if (!circuitArea.contains(e.getX(), e.getY()))
			return;

		dragElm = constructElement(mouseModeStr, x0, y0);
	}

	void doMainMenuChecks() {
		int c = mainMenuItems.size();
		int i;
		for (i = 0; i < c; i++)
			mainMenuItems.get(i).setState(mainMenuItemNames.get(i) == mouseModeStr);
	}

	public void onMouseUp(MouseUpEvent e) {
		e.preventDefault();
		mouseDragging = false;

		// click to clear selection
		if (tempMouseMode == MODE_SELECT && selectedArea == null)
			clearSelection();

		// cmd-click = split wire
		if (tempMouseMode == MODE_DRAG_POST && draggingPost == -1)
			doSplit(mouseElm);

		tempMouseMode = mouseMode;
		selectedArea = null;
		dragging = false;
		boolean circuitChanged = false;
		if (heldSwitchElm != null) {
			heldSwitchElm.mouseUp();
			heldSwitchElm = null;
			circuitChanged = true;
		}
		if (dragElm != null) {
			// if the element is zero size then don't create it
			// IES - and disable any previous selection
			if (dragElm.creationFailed()) {
				dragElm.delete();
				if (mouseMode == MODE_SELECT || mouseMode == MODE_DRAG_SELECTED)
					clearSelection();
			} else {
				mElmList.addElement(dragElm);
				dragElm.draggingDone();
				circuitChanged = true;
				writeRecoveryToStorage();
			}
			dragElm = null;
		}
		if (circuitChanged)
			needAnalyze();
		if (dragElm != null)
			dragElm.delete();
		dragElm = null;
		repaint();
	}

	public void onMouseWheel(MouseWheelEvent e) {
		e.preventDefault();

		// once we start zooming, don't allow other uses of mouse wheel for a while
		// so we don't accidentally edit a resistor value while zooming
		boolean zoomOnly = System.currentTimeMillis() < zoomTime + 1000;

		if (!zoomOnly)
			scrollValues(e.getNativeEvent().getClientX(), e.getNativeEvent().getClientY(), e.getDeltaY());

		if (mouseElm instanceof MouseWheelHandler && !zoomOnly)
			((MouseWheelHandler) mouseElm).onMouseWheel(e);
		else if (scopeSelected != -1)
			mCirManager.mScopes[scopeSelected].onMouseWheel(e);
		else if (!dialogIsShowing()) {
			zoomCircuit(e.getDeltaY());
			zoomTime = System.currentTimeMillis();
		}
		repaint();
	}

	void zoomCircuit(int dy) {
		double newScale;
		double oldScale = transform[0];
		double val = dy * .01;
		newScale = Math.max(oldScale + val, .2);
		newScale = Math.min(newScale, 2.5);
		setCircuitScale(newScale);
	}

	void setCircuitScale(double newScale) {
		int cx = inverseTransformX(circuitArea.width / 2);
		int cy = inverseTransformY(circuitArea.height / 2);
		transform[0] = transform[3] = newScale;

		// adjust translation to keep center of screen constant
		// inverse transform = (x-t4)/t0
		transform[4] = circuitArea.width / 2 - cx * newScale;
		transform[5] = circuitArea.height / 2 - cy * newScale;
	}

	void setPowerBarEnable() {
		if (powerCheckItem.getState()) {
			powerLabel.setStyleName("disabled", false);
			powerBar.enable();
		} else {
			powerLabel.setStyleName("disabled", true);
			powerBar.disable();
		}
	}

	void scrollValues(int x, int y, int deltay) {
		if (mouseElm != null && !dialogIsShowing() && scopeSelected == -1)
			if (mouseElm instanceof ResistorElm || mouseElm instanceof CapacitorElm
					|| mouseElm instanceof InductorElm) {
				scrollValuePopup = new ScrollValuePopup(x, y, deltay, mouseElm, this);
			}
	}

	void enableItems() {
	}

	void setGrid() {
		gridSize = (smallGridCheckItem.getState()) ? 8 : 16;
		gridMask = ~(gridSize - 1);
		gridRound = gridSize / 2 - 1;
	}

	void pushUndo() {
		redoStack.removeAllElements();
		String s = dumpCircuit();
		if (undoStack.size() > 0 && s.compareTo(undoStack.lastElement()) == 0)
			return;
		undoStack.add(s);
		enableUndoRedo();
	}

	void doUndo() {
		if (undoStack.size() == 0)
			return;
		redoStack.add(dumpCircuit());
		String s = undoStack.remove(undoStack.size() - 1);
		readSetup(s, false);
		enableUndoRedo();
	}

	void doRedo() {
		if (redoStack.size() == 0)
			return;
		undoStack.add(dumpCircuit());
		String s = redoStack.remove(redoStack.size() - 1);
		readSetup(s, false);
		enableUndoRedo();
	}

	void doRecover() {
		pushUndo();
		readSetup(recovery, false);
		recoverItem.setEnabled(false);
	}

	void enableUndoRedo() {
		redoItem.setEnabled(redoStack.size() > 0);
		undoItem.setEnabled(undoStack.size() > 0);
	}

	void setMouseMode(int mode) {
		mouseMode = mode;
		if (mode == MODE_ADD_ELM) {
			setCursorStyle("cursorCross");
		} else {
			setCursorStyle("cursorPointer");
		}
	}

	void setCursorStyle(String s) {
		if (lastCursorStyle != null)
			cv.removeStyleName(lastCursorStyle);
		cv.addStyleName(s);
		lastCursorStyle = s;
	}

	void setMenuSelection() {
		if (menuElm != null) {
			if (menuElm.selected)
				return;
			clearSelection();
			menuElm.setSelected(true);
		}
	}

	void doCut() {
		int i;
		pushUndo();
		setMenuSelection();
		clipboard = "";
		for (i = mElmList.size() - 1; i >= 0; i--) {
			CircuitElm ce = getElm(i);
			// ScopeElms don't cut-paste well because their reference to a parent
			// elm by number get's messed up in the dump. For now we will just ignore them
			// until I can be bothered to come up with something better
			if (willDelete(ce) && !(ce instanceof ScopeElm)) {
				clipboard += ce.dump() + "\n";
			}
		}
		writeClipboardToStorage();
		doDelete(true);
		enablePaste();
	}

	void writeClipboardToStorage() {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		stor.setItem("circuitClipboard", clipboard);
	}

	void readClipboardFromStorage() {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		clipboard = stor.getItem("circuitClipboard");
	}

	void writeRecoveryToStorage() {
		console("write recovery");
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		String s = dumpCircuit();
		stor.setItem("circuitRecovery", s);
	}

	void readRecovery() {
		Storage stor = Storage.getLocalStorageIfSupported();
		if (stor == null)
			return;
		recovery = stor.getItem("circuitRecovery");
	}

	void deleteUnusedScopeElms() {
		// Remove any scopeElms for elements that no longer exist
		for (int i = mElmList.size() - 1; i >= 0; i--) {
			CircuitElm ce = getElm(i);
			if (ce instanceof ScopeElm && (((ScopeElm) ce).elmScope.needToRemove())) {
				ce.delete();
				mElmList.removeElementAt(i);
			}
		}
	}

	void doDelete(boolean pushUndoFlag) {
		int i;
		if (pushUndoFlag)
			pushUndo();
		boolean hasDeleted = false;

		for (i = mElmList.size() - 1; i >= 0; i--) {
			CircuitElm ce = getElm(i);
			if (willDelete(ce)) {
				if (ce.isMouseElm())
					setMouseElm(null);
				ce.delete();
				mElmList.removeElementAt(i);
				hasDeleted = true;
			}
		}
		if (hasDeleted) {
			deleteUnusedScopeElms();
			needAnalyze();
			writeRecoveryToStorage();
		}
	}

	boolean willDelete(CircuitElm ce) {
		// Is this element in the list to be deleted.
		// This changes the logic from the previous version which would initially only
		// delete selected elements (which could include the mouseElm) and then delete
		// the
		// mouseElm if there were no selected elements. Not really sure this added
		// anything useful
		// to the user experience.
		//
		// BTW, the old logic could also leave mouseElm pointing to a deleted element.
		return ce.isSelected() || ce.isMouseElm();
	}

	String copyOfSelectedElms() {
		String r = "";
		CustomLogicModel.clearDumpedFlags();
		CustomCompositeModel.clearDumpedFlags();
		DiodeModel.clearDumpedFlags();
		for (int i = mElmList.size() - 1; i >= 0; i--) {
			CircuitElm ce = getElm(i);
			String m = ce.dumpModel();
			if (m != null && !m.isEmpty())
				r += m + "\n";
			// See notes on do cut why we don't copy ScopeElms.
			if (ce.isSelected() && !(ce instanceof ScopeElm))
				r += ce.dump() + "\n";
		}
		return r;
	}

	void doCopy() {
		// clear selection when we're done if we're copying a single element using the
		// context menu
		boolean clearSel = (menuElm != null && !menuElm.selected);

		setMenuSelection();
		clipboard = copyOfSelectedElms();

		if (clearSel)
			clearSelection();

		writeClipboardToStorage();
		enablePaste();
	}

	void enablePaste() {
		if (clipboard == null || clipboard.length() == 0)
			readClipboardFromStorage();
		pasteItem.setEnabled(clipboard != null && clipboard.length() > 0);
	}

	void doDuplicate() {
		String s;
		setMenuSelection();
		s = copyOfSelectedElms();
		doPaste(s);
	}

	void doPaste(String dump) {
		pushUndo();
		clearSelection();
		int i;
		Rectangle oldbb = null;

		// get old bounding box
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			Rectangle bb = ce.getBoundingBox();
			if (oldbb != null)
				oldbb = oldbb.union(bb);
			else
				oldbb = bb;
		}

		// add new items
		int oldsz = mElmList.size();
		if (dump != null)
			readSetup(dump, true, false);
		else {
			readClipboardFromStorage();
			readSetup(clipboard, true, false);
		}

		// select new items and get their bounding box
		Rectangle newbb = null;
		for (i = oldsz; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			ce.setSelected(true);
			Rectangle bb = ce.getBoundingBox();
			if (newbb != null)
				newbb = newbb.union(bb);
			else
				newbb = bb;
		}

		if (oldbb != null && newbb != null && oldbb.intersects(newbb)) {
			// find a place on the edge for new items
			int dx = 0, dy = 0;
			int spacew = circuitArea.width - oldbb.width - newbb.width;
			int spaceh = circuitArea.height - oldbb.height - newbb.height;
			if (spacew > spaceh)
				dx = snapGrid(oldbb.x + oldbb.width - newbb.x + gridSize);
			else
				dy = snapGrid(oldbb.y + oldbb.height - newbb.y + gridSize);

			// move new items near the mouse if possible
			if (mouseCursorX > 0 && circuitArea.contains(mouseCursorX, mouseCursorY)) {
				int gx = inverseTransformX(mouseCursorX);
				int gy = inverseTransformY(mouseCursorY);
				int mdx = snapGrid(gx - (newbb.x + newbb.width / 2));
				int mdy = snapGrid(gy - (newbb.y + newbb.height / 2));
				for (i = oldsz; i != mElmList.size(); i++) {
					if (!getElm(i).allowMove(mdx, mdy))
						break;
				}
				if (i == mElmList.size()) {
					dx = mdx;
					dy = mdy;
				}
			}

			// move the new items
			for (i = oldsz; i != mElmList.size(); i++) {
				CircuitElm ce = getElm(i);
				ce.move(dx, dy);
			}

			// center circuit
			// handleResize();
		}
		needAnalyze();
		writeRecoveryToStorage();
	}

	void clearSelection() {
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			ce.setSelected(false);
		}
	}

	void doSelectAll() {
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			ce.setSelected(true);
		}
	}

	boolean anySelectedButMouse() {
		for (int i = 0; i != mElmList.size(); i++)
			if (getElm(i) != mouseElm && getElm(i).selected)
				return true;
		return false;
	}

	// public void keyPressed(KeyEvent e) {}
	// public void keyReleased(KeyEvent e) {}

	boolean dialogIsShowing() {
		if (editDialog != null && editDialog.isShowing())
			return true;
		if (sliderDialog != null && sliderDialog.isShowing())
			return true;
		if (customLogicEditDialog != null && customLogicEditDialog.isShowing())
			return true;
		if (diodeModelEditDialog != null && diodeModelEditDialog.isShowing())
			return true;
		if (dialogShowing != null && dialogShowing.isShowing())
			return true;
		if (contextPanel != null && contextPanel.isShowing())
			return true;
		if (scrollValuePopup != null && scrollValuePopup.isShowing())
			return true;
		if (aboutBox != null && aboutBox.isShowing())
			return true;
		if (importFromDropboxDialog != null && importFromDropboxDialog.isShowing())
			return true;
		return false;
	}

	public void onPreviewNativeEvent(NativePreviewEvent e) {
		int cc = e.getNativeEvent().getCharCode();
		int t = e.getTypeInt();
		int code = e.getNativeEvent().getKeyCode();
		if (dialogIsShowing()) {
			if (scrollValuePopup != null && scrollValuePopup.isShowing() && (t & Event.ONKEYDOWN) != 0) {
				if (code == KEY_ESCAPE || code == KEY_SPACE)
					scrollValuePopup.close(false);
				if (code == KEY_ENTER)
					scrollValuePopup.close(true);
			}
			if (editDialog != null && editDialog.isShowing() && (t & Event.ONKEYDOWN) != 0) {
				if (code == KEY_ESCAPE)
					editDialog.closeDialog();
				if (code == KEY_ENTER) {
					editDialog.apply();
					editDialog.closeDialog();
				}
			}
			return;
		}
		if ((t & Event.ONKEYDOWN) != 0) {
			if (code == KEY_BACKSPACE || code == KEY_DELETE) {
				if (scopeSelected != -1) {
					// Treat DELETE key with scope selected as "remove scope", not delete
					mCirManager.mScopes[scopeSelected].setElm(null);
					scopeSelected = -1;
				} else {
					menuElm = null;
					pushUndo();
					doDelete(true);
					e.cancel();
				}
			}
			if (code == KEY_ESCAPE) {
				setMouseMode(MODE_SELECT);
				mouseModeStr = "Select";
				tempMouseMode = mouseMode;
				e.cancel();
			}

			if (e.getNativeEvent().getCtrlKey() || e.getNativeEvent().getMetaKey()) {
				if (code == KEY_C) {
					menuPerformed("key", "copy");
					e.cancel();
				}
				if (code == KEY_X) {
					menuPerformed("key", "cut");
					e.cancel();
				}
				if (code == KEY_V) {
					menuPerformed("key", "paste");
					e.cancel();
				}
				if (code == KEY_Z) {
					menuPerformed("key", "undo");
					e.cancel();
				}
				if (code == KEY_Y) {
					menuPerformed("key", "redo");
					e.cancel();
				}
				if (code == KEY_D) {
					menuPerformed("key", "duplicate");
					e.cancel();
				}
				if (code == KEY_A) {
					menuPerformed("key", "selectAll");
					e.cancel();
				}
			}
		}
		if ((t & Event.ONKEYPRESS) != 0) {
			if (cc == '-') {
				menuPerformed("key", "zoomout");
				e.cancel();
			}
			if (cc == '+' || cc == '=') {
				menuPerformed("key", "zoomin");
				e.cancel();
			}
			if (cc == '0') {
				menuPerformed("key", "zoom100");
				e.cancel();
			}

			if (cc > 32 && cc < 127) {
				String c = shortcuts[cc];
				e.cancel();
				if (c == null)
					return;
				setMouseMode(MODE_ADD_ELM);
				mouseModeStr = c;
				tempMouseMode = mouseMode;
			}
			if (cc == 32) {
				setMouseMode(MODE_SELECT);
				mouseModeStr = "Select";
				tempMouseMode = mouseMode;
				e.cancel();
			}
		}
	}

	void createNewLoadFile() {
		// This is a hack to fix what IMHO is a bug in the <INPUT FILE element
		// reloading the same file doesn't create a change event so importing the same
		// file twice
		// doesn't work unless you destroy the original input element and replace it
		// with a new one
		int idx = verticalPanel.getWidgetIndex(loadFileInput);
		LoadFile newlf = new LoadFile(this);
		verticalPanel.insert(newlf, idx);
		verticalPanel.remove(idx + 1);
		loadFileInput = newlf;
	}

	void addWidgetToVerticalPanel(Widget w) {
		if (iFrame != null) {
			int i = verticalPanel.getWidgetIndex(iFrame);
			verticalPanel.insert(w, i);
			setiFrameHeight();
		} else
			verticalPanel.add(w);
	}

	void removeWidgetFromVerticalPanel(Widget w) {
		verticalPanel.remove(w);
		if (iFrame != null)
			setiFrameHeight();
	}

	public static CircuitElm createCe(int tint, int x1, int y1, int x2, int y2, int f, StringTokenizer st) {
		if (tint == 'g')
			return (CircuitElm) new GroundElm(x1, y1, x2, y2, f, st);
		if (tint == 'r')
			return (CircuitElm) new ResistorElm(x1, y1, x2, y2, f, st);
		if (tint == 'R')
			return (CircuitElm) new RailElm(x1, y1, x2, y2, f, st);
		if (tint == 's')
			return (CircuitElm) new SwitchElm(x1, y1, x2, y2, f, st);
		if (tint == 'S')
			return (CircuitElm) new Switch2Elm(x1, y1, x2, y2, f, st);
		if (tint == 't')
			return (CircuitElm) new TransistorElm(x1, y1, x2, y2, f, st);
		if (tint == 'w')
			return (CircuitElm) new WireElm(x1, y1, x2, y2, f, st);
		if (tint == 'c')
			return (CircuitElm) new CapacitorElm(x1, y1, x2, y2, f, st);
		if (tint == 209)
			return (CircuitElm) new PolarCapacitorElm(x1, y1, x2, y2, f, st);
		if (tint == 'l')
			return (CircuitElm) new InductorElm(x1, y1, x2, y2, f, st);
		if (tint == 'v')
			return (CircuitElm) new VoltageElm(x1, y1, x2, y2, f, st);
		if (tint == 172)
			return (CircuitElm) new VarRailElm(x1, y1, x2, y2, f, st);
		if (tint == 174)
			return (CircuitElm) new PotElm(x1, y1, x2, y2, f, st);
		if (tint == 'O')
			return (CircuitElm) new OutputElm(x1, y1, x2, y2, f, st);
		if (tint == 'i')
			return (CircuitElm) new CurrentElm(x1, y1, x2, y2, f, st);
		if (tint == 'p')
			return (CircuitElm) new ProbeElm(x1, y1, x2, y2, f, st);
		if (tint == 'd')
			return (CircuitElm) new DiodeElm(x1, y1, x2, y2, f, st);
		if (tint == 'z')
			return (CircuitElm) new ZenerElm(x1, y1, x2, y2, f, st);
		if (tint == 170)
			return (CircuitElm) new SweepElm(x1, y1, x2, y2, f, st);
		if (tint == 162)
			return (CircuitElm) new LEDElm(x1, y1, x2, y2, f, st);
		if (tint == 'A')
			return (CircuitElm) new AntennaElm(x1, y1, x2, y2, f, st);
		if (tint == 'L')
			return (CircuitElm) new LogicInputElm(x1, y1, x2, y2, f, st);
		if (tint == 'M')
			return (CircuitElm) new LogicOutputElm(x1, y1, x2, y2, f, st);
		if (tint == 'T')
			return (CircuitElm) new TransformerElm(x1, y1, x2, y2, f, st);
		if (tint == 169)
			return (CircuitElm) new TappedTransformerElm(x1, y1, x2, y2, f, st);
		if (tint == 171)
			return (CircuitElm) new TransLineElm(x1, y1, x2, y2, f, st);
		if (tint == 178)
			return (CircuitElm) new RelayElm(x1, y1, x2, y2, f, st);
		if (tint == 'm')
			return (CircuitElm) new MemristorElm(x1, y1, x2, y2, f, st);
		if (tint == 187)
			return (CircuitElm) new SparkGapElm(x1, y1, x2, y2, f, st);
		if (tint == 200)
			return (CircuitElm) new AMElm(x1, y1, x2, y2, f, st);
		if (tint == 201)
			return (CircuitElm) new FMElm(x1, y1, x2, y2, f, st);
		if (tint == 'n')
			return (CircuitElm) new NoiseElm(x1, y1, x2, y2, f, st);
		if (tint == 181)
			return (CircuitElm) new LampElm(x1, y1, x2, y2, f, st);
		if (tint == 'a')
			return (CircuitElm) new OpAmpElm(x1, y1, x2, y2, f, st);
		if (tint == 'f')
			return (CircuitElm) new MosfetElm(x1, y1, x2, y2, f, st);
		if (tint == 'j')
			return (CircuitElm) new JfetElm(x1, y1, x2, y2, f, st);
		if (tint == 159)
			return (CircuitElm) new AnalogSwitchElm(x1, y1, x2, y2, f, st);
		if (tint == 160)
			return (CircuitElm) new AnalogSwitch2Elm(x1, y1, x2, y2, f, st);
		if (tint == 180)
			return (CircuitElm) new TriStateElm(x1, y1, x2, y2, f, st);
		if (tint == 182)
			return (CircuitElm) new SchmittElm(x1, y1, x2, y2, f, st);
		if (tint == 183)
			return (CircuitElm) new InvertingSchmittElm(x1, y1, x2, y2, f, st);
		if (tint == 177)
			return (CircuitElm) new SCRElm(x1, y1, x2, y2, f, st);
		if (tint == 203)
			return (CircuitElm) new DiacElm(x1, y1, x2, y2, f, st);
		if (tint == 206)
			return (CircuitElm) new TriacElm(x1, y1, x2, y2, f, st);
		if (tint == 173)
			return (CircuitElm) new TriodeElm(x1, y1, x2, y2, f, st);
		if (tint == 175)
			return (CircuitElm) new TunnelDiodeElm(x1, y1, x2, y2, f, st);
		if (tint == 176)
			return (CircuitElm) new VaractorElm(x1, y1, x2, y2, f, st);
		if (tint == 179)
			return (CircuitElm) new CC2Elm(x1, y1, x2, y2, f, st);
		if (tint == 'I')
			return (CircuitElm) new InverterElm(x1, y1, x2, y2, f, st);
		if (tint == 151)
			return (CircuitElm) new NandGateElm(x1, y1, x2, y2, f, st);
		if (tint == 153)
			return (CircuitElm) new NorGateElm(x1, y1, x2, y2, f, st);
		if (tint == 150)
			return (CircuitElm) new AndGateElm(x1, y1, x2, y2, f, st);
		if (tint == 152)
			return (CircuitElm) new OrGateElm(x1, y1, x2, y2, f, st);
		if (tint == 154)
			return (CircuitElm) new XorGateElm(x1, y1, x2, y2, f, st);
		if (tint == 155)
			return (CircuitElm) new DFlipFlopElm(x1, y1, x2, y2, f, st);
		if (tint == 156)
			return (CircuitElm) new JKFlipFlopElm(x1, y1, x2, y2, f, st);
		if (tint == 157)
			return (CircuitElm) new SevenSegElm(x1, y1, x2, y2, f, st);
		if (tint == 184)
			return (CircuitElm) new MultiplexerElm(x1, y1, x2, y2, f, st);
		if (tint == 185)
			return (CircuitElm) new DeMultiplexerElm(x1, y1, x2, y2, f, st);
		if (tint == 189)
			return (CircuitElm) new SipoShiftElm(x1, y1, x2, y2, f, st);
		if (tint == 186)
			return (CircuitElm) new PisoShiftElm(x1, y1, x2, y2, f, st);
		if (tint == 161)
			return (CircuitElm) new PhaseCompElm(x1, y1, x2, y2, f, st);
		if (tint == 164)
			return (CircuitElm) new CounterElm(x1, y1, x2, y2, f, st);
		if (tint == 163)
			return (CircuitElm) new RingCounterElm(x1, y1, x2, y2, f, st);
		if (tint == 165)
			return (CircuitElm) new TimerElm(x1, y1, x2, y2, f, st);
		if (tint == 166)
			return (CircuitElm) new DACElm(x1, y1, x2, y2, f, st);
		if (tint == 167)
			return (CircuitElm) new ADCElm(x1, y1, x2, y2, f, st);
		if (tint == 168)
			return (CircuitElm) new LatchElm(x1, y1, x2, y2, f, st);
		if (tint == 188)
			return (CircuitElm) new SeqGenElm(x1, y1, x2, y2, f, st);
		if (tint == 158)
			return (CircuitElm) new VCOElm(x1, y1, x2, y2, f, st);
		if (tint == 'b')
			return (CircuitElm) new BoxElm(x1, y1, x2, y2, f, st);
		if (tint == 'x')
			return (CircuitElm) new TextElm(x1, y1, x2, y2, f, st);
		if (tint == 193)
			return (CircuitElm) new TFlipFlopElm(x1, y1, x2, y2, f, st);
		if (tint == 197)
			return (CircuitElm) new SevenSegDecoderElm(x1, y1, x2, y2, f, st);
		if (tint == 196)
			return (CircuitElm) new FullAdderElm(x1, y1, x2, y2, f, st);
		if (tint == 195)
			return (CircuitElm) new HalfAdderElm(x1, y1, x2, y2, f, st);
		if (tint == 194)
			return (CircuitElm) new MonostableElm(x1, y1, x2, y2, f, st);
		if (tint == 207)
			return (CircuitElm) new LabeledNodeElm(x1, y1, x2, y2, f, st);
		if (tint == 208)
			return (CircuitElm) new CustomLogicElm(x1, y1, x2, y2, f, st);
		if (tint == 210)
			return (CircuitElm) new DataRecorderElm(x1, y1, x2, y2, f, st);
		if (tint == 211)
			return (CircuitElm) new AudioOutputElm(x1, y1, x2, y2, f, st);
		if (tint == 212)
			return (CircuitElm) new VCVSElm(x1, y1, x2, y2, f, st);
		if (tint == 213)
			return (CircuitElm) new VCCSElm(x1, y1, x2, y2, f, st);
		if (tint == 214)
			return (CircuitElm) new CCVSElm(x1, y1, x2, y2, f, st);
		if (tint == 215)
			return (CircuitElm) new CCCSElm(x1, y1, x2, y2, f, st);
		if (tint == 216)
			return (CircuitElm) new OhmMeterElm(x1, y1, x2, y2, f, st);
		if (tint == 368)
			return new TestPointElm(x1, y1, x2, y2, f, st);
		if (tint == 370)
			return new AmmeterElm(x1, y1, x2, y2, f, st);
		if (tint == 400)
			return new DarlingtonElm(x1, y1, x2, y2, f, st);
		if (tint == 401)
			return new ComparatorElm(x1, y1, x2, y2, f, st);
		if (tint == 402)
			return new OTAElm(x1, y1, x2, y2, f, st);
		if (tint == 403)
			return new ScopeElm(x1, y1, x2, y2, f, st);
		if (tint == 404)
			return new FuseElm(x1, y1, x2, y2, f, st);
		if (tint == 405)
			return new LEDArrayElm(x1, y1, x2, y2, f, st);
		if (tint == 406)
			return new CustomTransformerElm(x1, y1, x2, y2, f, st);
		if (tint == 407)
			return new OptocouplerElm(x1, y1, x2, y2, f, st);
		if (tint == 408)
			return new StopTriggerElm(x1, y1, x2, y2, f, st);
		if (tint == 409)
			return new OpAmpRealElm(x1, y1, x2, y2, f, st);
		if (tint == 410)
			return new CustomCompositeElm(x1, y1, x2, y2, f, st);
		if (tint == 411)
			return new AudioInputElm(x1, y1, x2, y2, f, st);
		return null;
	}

	public static CircuitElm constructElement(String n, int x1, int y1) {
		if (n == "GroundElm")
			return (CircuitElm) new GroundElm(x1, y1);
		if (n == "ResistorElm")
			return (CircuitElm) new ResistorElm(x1, y1);
		if (n == "RailElm")
			return (CircuitElm) new RailElm(x1, y1);
		if (n == "SwitchElm")
			return (CircuitElm) new SwitchElm(x1, y1);
		if (n == "Switch2Elm")
			return (CircuitElm) new Switch2Elm(x1, y1);
		if (n == "NTransistorElm" || n == "TransistorElm")
			return (CircuitElm) new NTransistorElm(x1, y1);
		if (n == "PTransistorElm")
			return (CircuitElm) new PTransistorElm(x1, y1);
		if (n == "WireElm")
			return (CircuitElm) new WireElm(x1, y1);
		if (n == "CapacitorElm")
			return (CircuitElm) new CapacitorElm(x1, y1);
		if (n == "PolarCapacitorElm")
			return (CircuitElm) new PolarCapacitorElm(x1, y1);
		if (n == "InductorElm")
			return (CircuitElm) new InductorElm(x1, y1);
		if (n == "DCVoltageElm" || n == "VoltageElm")
			return (CircuitElm) new DCVoltageElm(x1, y1);
		if (n == "VarRailElm")
			return (CircuitElm) new VarRailElm(x1, y1);
		if (n == "PotElm")
			return (CircuitElm) new PotElm(x1, y1);
		if (n == "OutputElm")
			return (CircuitElm) new OutputElm(x1, y1);
		if (n == "CurrentElm")
			return (CircuitElm) new CurrentElm(x1, y1);
		if (n == "ProbeElm")
			return (CircuitElm) new ProbeElm(x1, y1);
		if (n == "DiodeElm")
			return (CircuitElm) new DiodeElm(x1, y1);
		if (n == "ZenerElm")
			return (CircuitElm) new ZenerElm(x1, y1);
		if (n == "ACVoltageElm")
			return (CircuitElm) new ACVoltageElm(x1, y1);
		if (n == "ACRailElm")
			return (CircuitElm) new ACRailElm(x1, y1);
		if (n == "SquareRailElm")
			return (CircuitElm) new SquareRailElm(x1, y1);
		if (n == "SweepElm")
			return (CircuitElm) new SweepElm(x1, y1);
		if (n == "LEDElm")
			return (CircuitElm) new LEDElm(x1, y1);
		if (n == "AntennaElm")
			return (CircuitElm) new AntennaElm(x1, y1);
		if (n == "LogicInputElm")
			return (CircuitElm) new LogicInputElm(x1, y1);
		if (n == "LogicOutputElm")
			return (CircuitElm) new LogicOutputElm(x1, y1);
		if (n == "TransformerElm")
			return (CircuitElm) new TransformerElm(x1, y1);
		if (n == "TappedTransformerElm")
			return (CircuitElm) new TappedTransformerElm(x1, y1);
		if (n == "TransLineElm")
			return (CircuitElm) new TransLineElm(x1, y1);
		if (n == "RelayElm")
			return (CircuitElm) new RelayElm(x1, y1);
		if (n == "MemristorElm")
			return (CircuitElm) new MemristorElm(x1, y1);
		if (n == "SparkGapElm")
			return (CircuitElm) new SparkGapElm(x1, y1);
		if (n == "ClockElm")
			return (CircuitElm) new ClockElm(x1, y1);
		if (n == "AMElm")
			return (CircuitElm) new AMElm(x1, y1);
		if (n == "FMElm")
			return (CircuitElm) new FMElm(x1, y1);
		if (n == "LampElm")
			return (CircuitElm) new LampElm(x1, y1);
		if (n == "PushSwitchElm")
			return (CircuitElm) new PushSwitchElm(x1, y1);
		if (n == "OpAmpElm")
			return (CircuitElm) new OpAmpElm(x1, y1);
		if (n == "OpAmpSwapElm")
			return (CircuitElm) new OpAmpSwapElm(x1, y1);
		if (n == "NMosfetElm" || n == "MosfetElm")
			return (CircuitElm) new NMosfetElm(x1, y1);
		if (n == "PMosfetElm")
			return (CircuitElm) new PMosfetElm(x1, y1);
		if (n == "NJfetElm" || n == "JfetElm")
			return (CircuitElm) new NJfetElm(x1, y1);
		if (n == "PJfetElm")
			return (CircuitElm) new PJfetElm(x1, y1);
		if (n == "AnalogSwitchElm")
			return (CircuitElm) new AnalogSwitchElm(x1, y1);
		if (n == "AnalogSwitch2Elm")
			return (CircuitElm) new AnalogSwitch2Elm(x1, y1);
		if (n == "SchmittElm")
			return (CircuitElm) new SchmittElm(x1, y1);
		if (n == "InvertingSchmittElm")
			return (CircuitElm) new InvertingSchmittElm(x1, y1);
		if (n == "TriStateElm")
			return (CircuitElm) new TriStateElm(x1, y1);
		if (n == "SCRElm")
			return (CircuitElm) new SCRElm(x1, y1);
		if (n == "DiacElm")
			return (CircuitElm) new DiacElm(x1, y1);
		if (n == "TriacElm")
			return (CircuitElm) new TriacElm(x1, y1);
		if (n == "TriodeElm")
			return (CircuitElm) new TriodeElm(x1, y1);
		if (n == "VaractorElm")
			return (CircuitElm) new VaractorElm(x1, y1);
		if (n == "TunnelDiodeElm")
			return (CircuitElm) new TunnelDiodeElm(x1, y1);
		if (n == "CC2Elm")
			return (CircuitElm) new CC2Elm(x1, y1);
		if (n == "CC2NegElm")
			return (CircuitElm) new CC2NegElm(x1, y1);
		if (n == "InverterElm")
			return (CircuitElm) new InverterElm(x1, y1);
		if (n == "NandGateElm")
			return (CircuitElm) new NandGateElm(x1, y1);
		if (n == "NorGateElm")
			return (CircuitElm) new NorGateElm(x1, y1);
		if (n == "AndGateElm")
			return (CircuitElm) new AndGateElm(x1, y1);
		if (n == "OrGateElm")
			return (CircuitElm) new OrGateElm(x1, y1);
		if (n == "XorGateElm")
			return (CircuitElm) new XorGateElm(x1, y1);
		if (n == "DFlipFlopElm")
			return (CircuitElm) new DFlipFlopElm(x1, y1);
		if (n == "JKFlipFlopElm")
			return (CircuitElm) new JKFlipFlopElm(x1, y1);
		if (n == "SevenSegElm")
			return (CircuitElm) new SevenSegElm(x1, y1);
		if (n == "MultiplexerElm")
			return (CircuitElm) new MultiplexerElm(x1, y1);
		if (n == "DeMultiplexerElm")
			return (CircuitElm) new DeMultiplexerElm(x1, y1);
		if (n == "SipoShiftElm")
			return (CircuitElm) new SipoShiftElm(x1, y1);
		if (n == "PisoShiftElm")
			return (CircuitElm) new PisoShiftElm(x1, y1);
		if (n == "PhaseCompElm")
			return (CircuitElm) new PhaseCompElm(x1, y1);
		if (n == "CounterElm")
			return (CircuitElm) new CounterElm(x1, y1);
		if (n == "DecadeElm")
			return (CircuitElm) new RingCounterElm(x1, y1);
		if (n == "TimerElm")
			return (CircuitElm) new TimerElm(x1, y1);
		if (n == "DACElm")
			return (CircuitElm) new DACElm(x1, y1);
		if (n == "ADCElm")
			return (CircuitElm) new ADCElm(x1, y1);
		if (n == "LatchElm")
			return (CircuitElm) new LatchElm(x1, y1);
		if (n == "SeqGenElm")
			return (CircuitElm) new SeqGenElm(x1, y1);
		if (n == "VCOElm")
			return (CircuitElm) new VCOElm(x1, y1);
		if (n == "BoxElm")
			return (CircuitElm) new BoxElm(x1, y1);
		if (n == "TextElm")
			return (CircuitElm) new TextElm(x1, y1);
		if (n == "TFlipFlopElm")
			return (CircuitElm) new TFlipFlopElm(x1, y1);
		if (n == "SevenSegDecoderElm")
			return (CircuitElm) new SevenSegDecoderElm(x1, y1);
		if (n == "FullAdderElm")
			return (CircuitElm) new FullAdderElm(x1, y1);
		if (n == "HalfAdderElm")
			return (CircuitElm) new HalfAdderElm(x1, y1);
		if (n == "MonostableElm")
			return (CircuitElm) new MonostableElm(x1, y1);
		if (n == "LabeledNodeElm")
			return (CircuitElm) new LabeledNodeElm(x1, y1);

		// if you take out UserDefinedLogicElm, it will break people's saved shortcuts
		if (n == "UserDefinedLogicElm" || n == "CustomLogicElm")
			return (CircuitElm) new CustomLogicElm(x1, y1);

		if (n == "TestPointElm")
			return new TestPointElm(x1, y1);
		if (n == "AmmeterElm")
			return new AmmeterElm(x1, y1);
		if (n == "DataRecorderElm")
			return (CircuitElm) new DataRecorderElm(x1, y1);
		if (n == "AudioOutputElm")
			return (CircuitElm) new AudioOutputElm(x1, y1);
		if (n == "NDarlingtonElm" || n == "DarlingtonElm")
			return (CircuitElm) new NDarlingtonElm(x1, y1);
		if (n == "PDarlingtonElm")
			return (CircuitElm) new PDarlingtonElm(x1, y1);
		if (n == "ComparatorElm")
			return (CircuitElm) new ComparatorElm(x1, y1);
		if (n == "OTAElm")
			return (CircuitElm) new OTAElm(x1, y1);
		if (n == "NoiseElm")
			return (CircuitElm) new NoiseElm(x1, y1);
		if (n == "VCVSElm")
			return (CircuitElm) new VCVSElm(x1, y1);
		if (n == "VCCSElm")
			return (CircuitElm) new VCCSElm(x1, y1);
		if (n == "CCVSElm")
			return (CircuitElm) new CCVSElm(x1, y1);
		if (n == "CCCSElm")
			return (CircuitElm) new CCCSElm(x1, y1);
		if (n == "OhmMeterElm")
			return (CircuitElm) new OhmMeterElm(x1, y1);
		if (n == "ScopeElm")
			return (CircuitElm) new ScopeElm(x1, y1);
		if (n == "FuseElm")
			return (CircuitElm) new FuseElm(x1, y1);
		if (n == "LEDArrayElm")
			return (CircuitElm) new LEDArrayElm(x1, y1);
		if (n == "CustomTransformerElm")
			return (CircuitElm) new CustomTransformerElm(x1, y1);
		if (n == "OptocouplerElm")
			return (CircuitElm) new OptocouplerElm(x1, y1);
		if (n == "StopTriggerElm")
			return (CircuitElm) new StopTriggerElm(x1, y1);
		if (n == "OpAmpRealElm")
			return (CircuitElm) new OpAmpRealElm(x1, y1);
		if (n == "CustomCompositeElm")
			return (CircuitElm) new CustomCompositeElm(x1, y1);
		if (n == "AudioInputElm")
			return (CircuitElm) new AudioInputElm(x1, y1);
		return null;
	}

	public void updateModels() {
		int i;
		for (i = 0; i != mElmList.size(); i++)
			mElmList.get(i).updateModels();
	}

	native boolean weAreInUS() /*-{
								try {
								l = navigator.languages ? navigator.languages[0] : (navigator.language || navigator.userLanguage) ;  
								if (l.length > 2) {
								l = l.slice(-2).toUpperCase();
								return (l == "US" || l=="CA");
								} else {
								return 0;
								}
								
								} catch (e) { return 0;
								}
								}-*/;

	native boolean weAreInGermany() /*-{
									try {
									l = navigator.languages ? navigator.languages[0] : (navigator.language || navigator.userLanguage) ;
									return (l.toUpperCase().startsWith("DE"));
									} catch (e) { return 0;
									}
									}-*/;

	static String LS(String s) {
		if (s == null)
			return null;
		String sm = localizationMap.get(s);
		if (sm != null)
			return sm;

		// use trailing ~ to differentiate strings that are the same in English but need
		// different translations.
		// remove these if there's no translation.
		int ix = s.indexOf('~');
		if (ix < 0)
			return s;
		s = s.substring(0, ix);
		sm = localizationMap.get(s);
		if (sm != null)
			return sm;
		return s;
	}

	static SafeHtml LSHTML(String s) {
		return SafeHtmlUtils.fromTrustedString(LS(s));
	}

	// For debugging
	void dumpNodelist() {
		CircuitElm e;
		int i, j;
		String s;
		String cs;
		//
		// for(i=0; i<nodeList.size(); i++) {
		// s="Node "+i;
		// nd=nodeList.get(i);
		// for(j=0; j<nd.links.size();j++) {
		// s=s+" " + nd.links.get(j).num + " " +nd.links.get(j).elm.getDumpType();
		// }
		// console(s);
		// }
		console("Elm list Dump");
		for (i = 0; i < mElmList.size(); i++) {
			e = mElmList.get(i);
			cs = e.getDumpClass().toString();
			int p = cs.lastIndexOf('.');
			cs = cs.substring(p + 1);
			if (cs == "WireElm")
				continue;
			if (cs == "LabeledNodeElm")
				cs = cs + " " + ((LabeledNodeElm) e).text;
			if (cs == "TransistorElm") {
				if (((TransistorElm) e).pnp == -1)
					cs = "PTransistorElm";
				else
					cs = "NTransistorElm";
			}
			s = cs;
			for (j = 0; j < e.getPostCount(); j++) {
				s = s + " " + e.nodes[j];
			}
			console(s);
		}
	}

	native void printCanvas(CanvasElement cv) /*-{
												var img    = cv.toDataURL("image/png");
												var win = window.open("", "print", "height=500,width=500,status=yes,location=no");
												win.document.title = "Print Circuit";
												win.document.open();
												win.document.write('<img src="'+img+'"/>');
												win.document.close();
												setTimeout(function(){win.print();},1000);
												}-*/;

	void doDCAnalysis() {
		mDcAnalysisFlag = true;
		mCirManager.resetAction();
	}

	void doPrint() {
		Canvas cv = getCircuitAsCanvas(true);
		printCanvas(cv.getCanvasElement());
	}

	public Canvas getCircuitAsCanvas(boolean print) {
		// create canvas to draw circuit into
		Canvas cv = Canvas.createIfSupported();
		Rectangle bounds = getCircuitBounds();

		// add some space on edges because bounds calculation is not perfect
		int wmargin = 140;
		int hmargin = 100;
		int w = (bounds.width + wmargin);
		int h = (bounds.height + hmargin);
		cv.setCoordinateSpaceWidth(w);
		cv.setCoordinateSpaceHeight(h);
		double oldTransform[] = Arrays.copyOf(transform, 6);

		Context2d context = cv.getContext2d();
		Graphics g = new Graphics(context);
		context.setTransform(1, 0, 0, 1, 0, 0);

		double scale = 1;

		// turn on white background, turn off current display
		boolean p = printableCheckItem.getState();
		boolean c = dotsCheckItem.getState();
		if (print)
			printableCheckItem.setState(true);
		if (printableCheckItem.getState()) {
			CircuitElm.whiteColor = Color.black;
			CircuitElm.lightGrayColor = Color.black;
			g.setColor(Color.white);
		} else {
			CircuitElm.whiteColor = Color.white;
			CircuitElm.lightGrayColor = Color.lightGray;
			g.setColor(Color.black);
			g.fillRect(0, 0, g.context.getCanvas().getWidth(), g.context.getCanvas().getHeight());
		}
		dotsCheckItem.setState(false);

		if (bounds != null)
			scale = Math.min(w / (double) (bounds.width + wmargin), h / (double) (bounds.height + hmargin));
		scale = Math.min(scale, 1.5); // Limit scale so we don't create enormous circuits in big windows

		// ScopeElms need the transform array to be updated
		transform[0] = transform[3] = scale;
		transform[4] = -(bounds.x - wmargin / 2);
		transform[5] = -(bounds.y - hmargin / 2);
		context.scale(scale, scale);
		context.translate(transform[4], transform[5]);

		// draw elements
		int i;
		for (i = 0; i != mElmList.size(); i++) {
			getElm(i).draw(g);
		}
		for (i = 0; i != mCirManager.mPostDrawList.size(); i++) {
			CircuitElm.drawPost(g, mCirManager.mPostDrawList.get(i));
		}

		// restore everything
		printableCheckItem.setState(p);
		dotsCheckItem.setState(c);
		transform = oldTransform;
		return cv;
	}

	boolean isSelection() {
		for (int i = 0; i != mElmList.size(); i++)
			if (getElm(i).isSelected())
				return true;
		return false;
	}

	public CustomCompositeModel getCircuitAsComposite() {
		int i;
		String nodeList = "";
		String dump = "";
		// String models = "";
		CustomLogicModel.clearDumpedFlags();
		DiodeModel.clearDumpedFlags();
		Vector<ExtListEntry> extList = new Vector<ExtListEntry>();
		boolean sel = isSelection();

		// mapping of node labels -> node numbers
		HashMap<String, Integer> nodeNameHash = new HashMap<String, Integer>();

		// mapping of node numbers -> equivalent node numbers (if they both have the
		// same label)
		HashMap<Integer, Integer> nodeNumberHash = new HashMap<Integer, Integer>();

		// find all the labeled nodes, get a list of them, and create a node number map
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			if (sel && !ce.isSelected())
				continue;
			if (ce instanceof LabeledNodeElm) {
				LabeledNodeElm lne = (LabeledNodeElm) ce;
				String label = lne.text;
				Integer map = nodeNameHash.get(label);

				// this node name already seen? map the new node number to the old one
				if (map != null) {
					Integer val = nodeNumberHash.get(lne.getNode(0));
					if (val != null && !val.equals(map)) {
						Window.alert("Can't have a node with two labels!");
						return null;
					}
					nodeNumberHash.put(lne.getNode(0), map);
					continue;
				}
				nodeNameHash.put(label, lne.getNode(0));
				// put an entry in nodeNumberHash so we can detect if we try to map it to
				// something else later
				nodeNumberHash.put(lne.getNode(0), lne.getNode(0));
				if (lne.isInternal())
					continue;
				// create ext list entry for external nodes
				ExtListEntry ent = new ExtListEntry(label, ce.getNode(0));
				extList.add(ent);
			}
		}

		// output all the elements
		for (i = 0; i != mElmList.size(); i++) {
			CircuitElm ce = getElm(i);
			if (sel && !ce.isSelected())
				continue;
			// don't need these elements dumped
			if (ce instanceof WireElm || ce instanceof LabeledNodeElm || ce instanceof ScopeElm)
				continue;
			if (ce instanceof GraphicElm)
				continue;
			int j;
			if (nodeList.length() > 0)
				nodeList += "\r";
			nodeList += ce.getClass().getSimpleName();
			for (j = 0; j != ce.getPostCount(); j++) {
				int n = ce.getNode(j);
				Integer nobj = nodeNumberHash.get(n);
				int n0 = (nobj == null) ? n : nobj;
				nodeList += " " + n0;
			}

			// save positions
			int x1 = ce.x;
			int y1 = ce.y;
			int x2 = ce.x2;
			int y2 = ce.y2;

			// set them to 0 so they're easy to remove
			ce.x = ce.y = ce.x2 = ce.y2 = 0;

			String tstring = ce.dump();
			tstring = tstring.replaceFirst("[A-Za-z0-9]+ 0 0 0 0 ", ""); // remove unused tint_x1 y1 x2 y2 coords for
																			// internal components

			// restore positions
			ce.x = x1;
			ce.y = y1;
			ce.x2 = x2;
			ce.y2 = y2;
			if (dump.length() > 0)
				dump += " ";
			dump += CustomLogicModel.escape(tstring);
		}
		CustomCompositeModel ccm = new CustomCompositeModel();
		ccm.nodeList = nodeList;
		ccm.elmDump = dump;
		ccm.extList = extList;
		return ccm;
	}

	// *****************************************************************
	// UPDATE CIRCUIT
	public void updateCircuit() {
		long mystarttime;
		long myrunstarttime;
		long mydrawstarttime;
		// if (winSize == null || winSize.width == 0)
		// return;
		mystarttime = System.currentTimeMillis();
		boolean didAnalyze = mCirManager.mAnalyzeFlag;
		if (mCirManager.mAnalyzeFlag || mDcAnalysisFlag) {
			mCirManager.analyzeCircuit();
			mCirManager.mAnalyzeFlag = false;
		}
		// if (editDialog != null && editDialog.elm instanceof CircuitElm)
		// mouseElm = (CircuitElm) (editDialog.elm);
		if (mCirManager.mErrorElm != null && mCirManager.mErrorElm != mouseElm) {
			mCirManager.mErrorElm.setMouseElm(true);
		}
		mCirManager.setupScopes();

		Graphics g = new Graphics(backcontext);

		CircuitElm.selectColor = Color.cyan;
		if (printableCheckItem.getState()) {
			CircuitElm.whiteColor = Color.black;
			CircuitElm.lightGrayColor = Color.black;
			g.setColor(Color.white);
		} else {
			CircuitElm.whiteColor = Color.white;
			CircuitElm.lightGrayColor = Color.lightGray;
			g.setColor(Color.black);
		}
		g.fillRect(0, 0, g.context.getCanvas().getWidth(), g.context.getCanvas().getHeight());
		myrunstarttime = System.currentTimeMillis();
		if (mCirManager.simIsRunning()) {
			try {
				mCirManager.runCircuit(didAnalyze);
			} catch (Exception e) {
				debugger();
				console("exception in runCircuit " + e);
				e.printStackTrace();
				return;
			}
			myruntime += System.currentTimeMillis() - myrunstarttime;
		}
		long sysTime = System.currentTimeMillis();
		if (mCirManager.simIsRunning()) {
			if (mCirManager.mLastTime != 0) {
				int inc = (int) (sysTime - mCirManager.mLastTime);
				double c = currentBar.getValue();
				c = java.lang.Math.exp(c / 3.5 - 14.2);
				CircuitElm.currentMult = 1.7 * inc * c;
				if (!conventionCheckItem.getState())
					CircuitElm.currentMult = -CircuitElm.currentMult;
			}

			mCirManager.mLastTime = sysTime;
		} else {
			mCirManager.mLastTime = 0;
		}

		if (sysTime - mCirManager.mSecTime >= 1000) {
			mCirManager.mFramerate = mCirManager.mFrames;
			mCirManager.mSteprate = mCirManager.mSteps;
			mCirManager.mFrames = 0;
			mCirManager.mSteps = 0;
			mCirManager.mSecTime = sysTime;
		}
		CircuitElm.powerMult = Math.exp(powerBar.getValue() / 4.762 - 7);

		int i;
		// Font oldfont = g.getFont();
		Font oldfont = CircuitElm.unitsFont;
		g.setFont(oldfont);

		// this causes bad behavior on Chrome 55
		// g.clipRect(0, 0, circuitArea.width, circuitArea.height);

		mydrawstarttime = System.currentTimeMillis();

		g.context.setLineCap(LineCap.ROUND);

		// draw elements
		backcontext.setTransform(transform[0], transform[1], transform[2], transform[3], transform[4], transform[5]);
		for (i = 0; i != mElmList.size(); i++) {
			if (powerCheckItem.getState())
				g.setColor(Color.gray);
			/*
			 * else if (conductanceCheckItem.getState()) g.setColor(Color.white);
			 */
			getElm(i).draw(g);
		}
		mydrawtime += System.currentTimeMillis() - mydrawstarttime;

		// draw posts normally
		if (mouseMode != CirSim.MODE_DRAG_ROW && mouseMode != CirSim.MODE_DRAG_COLUMN) {
			for (i = 0; i != mCirManager.mPostDrawList.size(); i++)
				CircuitElm.drawPost(g, mCirManager.mPostDrawList.get(i));
		}

		// for some mouse modes, what matters is not the posts but the endpoints (which
		// are only
		// the same for 2-terminal elements). We draw those now if needed
		if (tempMouseMode == MODE_DRAG_ROW || tempMouseMode == MODE_DRAG_COLUMN || tempMouseMode == MODE_DRAG_POST
				|| tempMouseMode == MODE_DRAG_SELECTED)
			for (i = 0; i != mElmList.size(); i++) {

				CircuitElm ce = getElm(i);
				// ce.drawPost(g, ce.x , ce.y );
				// ce.drawPost(g, ce.x2, ce.y2);
				if (ce != mouseElm || tempMouseMode != MODE_DRAG_POST) {
					g.setColor(Color.gray);
					g.fillOval(ce.x - 3, ce.y - 3, 7, 7);
					g.fillOval(ce.x2 - 3, ce.y2 - 3, 7, 7);
				} else {
					ce.drawHandles(g, Color.cyan);
				}
			}
		// draw handles for elm we're creating
		if (tempMouseMode == MODE_SELECT && mouseElm != null) {
			mouseElm.drawHandles(g, Color.cyan);
		}

		// draw handles for elm we're dragging
		if (dragElm != null && (dragElm.x != dragElm.x2 || dragElm.y != dragElm.y2)) {
			dragElm.draw(g);
			dragElm.drawHandles(g, Color.cyan);
		}

		// draw bad connections. do this last so they will not be overdrawn.
		for (i = 0; i != mCirManager.mBadConnectionList.size(); i++) {
			Point cn = mCirManager.mBadConnectionList.get(i);
			g.setColor(Color.red);
			g.fillOval(cn.x - 3, cn.y - 3, 7, 7);
		}

		if (selectedArea != null) {
			g.setColor(CircuitElm.selectColor);
			g.drawRect(selectedArea.x, selectedArea.y, selectedArea.width, selectedArea.height);
		}

		if (crossHairCheckItem.getState() && mouseCursorX >= 0 && mouseCursorX <= circuitArea.width
				&& mouseCursorY <= circuitArea.height) {
			g.setColor(Color.gray);
			int x = snapGrid(inverseTransformX(mouseCursorX));
			int y = snapGrid(inverseTransformY(mouseCursorY));
			g.drawLine(x, inverseTransformY(0), x, inverseTransformY(circuitArea.height));
			g.drawLine(inverseTransformX(0), y, inverseTransformX(circuitArea.width), y);
		}

		backcontext.setTransform(1, 0, 0, 1, 0, 0);

		if (printableCheckItem.getState()) {
			g.setColor(Color.white);
		} else {
			g.setColor(Color.black);
		}
		g.fillRect(0, circuitArea.height, circuitArea.width, cv.getCoordinateSpaceHeight() - circuitArea.height);
		// g.restore();
		g.setFont(oldfont);
		int ct = mCirManager.mScopeCount;
		if (stopMessage != null) {
			ct = 0;
		}
		for (i = 0; i != ct; i++) {
			mCirManager.mScopes[i].draw(g);
		}
		if (mouseWasOverSplitter) {
			g.setColor(Color.cyan);
			g.setLineWidth(4.0);
			g.drawLine(0, circuitArea.height - 2, circuitArea.width, circuitArea.height - 2);
			g.setLineWidth(1.0);
		}
		g.setColor(CircuitElm.whiteColor);

		if (stopMessage != null) {
			g.drawString(stopMessage, 10, circuitArea.height - 10);
		} else {
			String info[] = new String[10];
			if (mouseElm != null) {
				if (mousePost == -1) {
					mouseElm.getInfo(info);
					info[0] = LS(info[0]);
					if (info[1] != null) {
						info[1] = LS(info[1]);
					}
				} else
					info[0] = "V = " + CircuitElm.getUnitText(mouseElm.getPostVoltage(mousePost), "V");
				// /* //shownodes
				// for (i = 0; i != mouseElm.getPostCount(); i++)
				// info[0] += " " + mouseElm.nodes[i];
				// if (mouseElm.getVoltageSourceCount() > 0)
				// info[0] += ";" + (mouseElm.getVoltageSource()+nodeList.size());
				// */
			} else {
				info[0] = "t = " + CircuitElm.getUnitText(mCirManager.mTime, "s");
				info[1] = LS("time step = ") + CircuitElm.getUnitText(mCirManager.mDeltaTime, "s");
			}
			if (hintType != -1) {
				for (i = 0; info[i] != null; i++) ;
				String s = getHint();
				if (s == null) {
					hintType = -1;
				} else {
					info[i] = s;
				}
			}
			int x = 0;
			if (ct != 0) {
				x = mCirManager.mScopes[ct - 1].rightEdge() + 20;
			}
			x = max(x, cv.getCoordinateSpaceWidth() * 2 / 3);
			// x=cv.getCoordinateSpaceWidth()*2/3;

			// count lines of data
			for (i = 0; info[i] != null; i++) ;
			int badnodes = mCirManager.mBadConnectionList.size();
			if (badnodes > 0) {
				info[i++] = badnodes + ((badnodes == 1) ? LS(" bad connection") : LS(" bad connections"));
			}
			int ybase = circuitArea.height;
			for (i = 0; info[i] != null; i++) {
				g.drawString(info[i], x, ybase + 15 * (i + 1));
			}
		}
		if (mCirManager.mErrorElm != null && mCirManager.mErrorElm != mouseElm) {
			mCirManager.mErrorElm.setMouseElm(false);
		}
		mCirManager.mFrames++;

		g.setColor(Color.white);
		// g.drawString("Framerate: " + CircuitElm.showFormat.format(framerate), 10,
		// 10);
		// g.drawString("Steprate: " + CircuitElm.showFormat.format(steprate), 10, 30);
		// g.drawString("Steprate/iter: " +
		// CircuitElm.showFormat.format(steprate/getIterCount()), 10, 50);
		// g.drawString("iterc: " + CircuitElm.showFormat.format(getIterCount()), 10,
		// 70);
		// g.drawString("Frames: "+ frames,10,90);
		// g.drawString("ms per frame (other): "+
		// CircuitElm.showFormat.format((mytime-myruntime-mydrawtime)/myframes),10,110);
		// g.drawString("ms per frame (sim): "+
		// CircuitElm.showFormat.format((myruntime)/myframes),10,130);
		// g.drawString("ms per frame (draw): "+
		// CircuitElm.showFormat.format((mydrawtime)/myframes),10,150);

		cvcontext.drawImage(backcontext.getCanvas(), 0.0, 0.0);

		// if we did DC analysis, we need to re-analyze the circuit with that flag
		// cleared.
		if (mDcAnalysisFlag) {
			mDcAnalysisFlag = false;
			mCirManager.mAnalyzeFlag = true;
		}

		mCirManager.mLastFrameTime = mCirManager.mLastTime;
		mMytime += System.currentTimeMillis() - mystarttime;
		mMyframes++;
	}
}
