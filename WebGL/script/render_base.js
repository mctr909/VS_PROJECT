/// <reference path="struct.js"/>
/// <reference path="queue.js"/>

class RenderBase extends Queue {
    /** @private */
    static _MODEL_LOAD      = "MODEL_LOAD";
    /** @private */
    static _MODEL_FETCH     = "MODEL_FETCH";
    /** @private */
    static _MODEL_PURGE     = "MODEL_PURGE";
    /** @private */
    static _MODEL_PURGE_ALL = "MODEL_PURGE_ALL";
    /** @private */
    static _MODEL_VISIBLE   = "MODEL_VISIBLE";
    /** @private */
    static _MODEL_POSTURE   = "MODEL_POSTURE";
    /** @private */
    static _MODEL_POSITION  = "MODEL_POSITION";
    /** @private */
    static _MODEL_BONE      = "MODEL_BONE";

    /**
     * @returns {string}
     */
    get Version() { return "RenderBase"; }

    /**
     * @returns {number}
     */
    get DeltaTime() { return (this._curTime - this._preTime)*0.001; }

    /**
     * @param {HTMLCanvasElement} canvasObj 
     */
    constructor(canvasObj) {
        super();
        /** @protected */
        this._gl = canvasObj.getContext("webgl") || canvasObj.getContext("experimental-webgl");
        /** @protected */
        this._modelList = new Array(ModelInfo);
        /** @protected */
        this._fetchModel = null;
        /** @private */
        this._curTime = (new Date()).getTime();
        /** @private */
        this._preTime = this._curTime;
        this._initialize();
    }

    update() {
        this._msgLoop();
        this._rendering();
        this._preTime = this._curTime;
        this._curTime = (new Date()).getTime();
    }

    /** @protected @virtual */
    _initialize() {}

    /** @protected @virtual */
    _rendering() {}

    /** @private */
    _msgLoop() {
        while(0 < this._que.length) {
            let msg = this._dequeue();
            switch(msg.Type) {
            case RenderBase._MODEL_LOAD:
                this._modelLoad(msg.Sender, msg.Value[0], msg.Value[1]);
                break;
            case RenderBase._MODEL_FETCH:
                this._modelFetch(msg.Sender, msg.Value[0], msg.Value[1]);
                break;
            case RenderBase._MODEL_PURGE:
                this._modelPurge(msg.Sender, msg.Value[0], msg.Value[1]);
                break;
            case RenderBase._MODEL_PURGE_ALL:
                this._modelPurgeAll(msg.Sender);
                break;
            case RenderBase._MODEL_VISIBLE:
                this._modelVisible(msg.Sender, msg.Value);
                break;
            case RenderBase._MODEL_POSTURE:
                this._modelPosture(msg.Sender, msg.Value);
                break;
            case RenderBase._MODEL_POSITION:
                this._modelPosition(msg.Sender, msg.Value);
                break;
            case RenderBase._MODEL_BONE:
                this._modelBone(msg.Sender, msg.Value);
                break;
            }
        }
    }

    /**
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    modelLoad(sender, id) {
        this._enqueue(RenderBase._MODEL_LOAD, sender, id, instanceId);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    _modelLoad(sender, id, instanceId) {}

    /**
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    modelFetch(sender, id, instanceId) {
        this._enqueue(RenderBase._MODEL_FETCH, sender, id, instanceId);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    _modelFetch(sender, id, instanceId) {}

    /**
     * @param {any} sender
     * @param {number} id
     * @param {string} instanceId
     */
    modelPurge(sender, id, instanceId) {
        this._enqueue(RenderBase._MODEL_PURGE, sender, id, instanceId);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {string} id
     * @param {string} instanceId
     */
    _modelPurge(sender, id, instanceId) {}

    /**
     * @param {any} sender
     */
    modelPurgeAll(sender) {
        this._enqueue(RenderBase._MODEL_PURGE_ALL, sender, null);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     */
    _modelPurgeAll(sender) {}

    /**
     * @param {any} sender
     * @param {number} alpha
     */
    modelVisible(sender, alpha) {
        this._enqueue(RenderBase._MODEL_VISIBLE, sender, alpha);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {number} alpha
     */
    _modelVisible(sender, alpha) {}

    /**
     * @param {any} sender
     * @param {Posture} posture
     */
    modelPosture(sender, posture) {
        this._enqueue(RenderBase._MODEL_POSTURE, sender, posture);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {Posture} posture
     */
    _modelPosture(sender, posture) {}

    /**
     * @param {any} sender
     * @param {Point} position
     */
    modelPosition(sender, position) {
        this._enqueue(RenderBase._MODEL_POSITION, sender, position);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {Point} position
     */
    _modelPosition(sender, position) {}

    /**
     * @param {any} sender
     * @param {BoneInfo[]} boneArray
     */
    modelBone(sender, boneArray) {
        this._enqueue(RenderBase._MODEL_BONE, sender, boneArray);
    }
    /**
     * @protected @virtual
     * @param {any} sender
     * @param {BoneInfo[]} boneArray
     */
    _modelBone(sender, boneArray) {}
}
