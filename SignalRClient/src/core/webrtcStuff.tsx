var adapter = require("webrtc-adapter")
export default class WebrtcStuff{

    constructor(onIceCandidate: (candidate: any) => void){
        this.sendTrickleCandidate = onIceCandidate;
    }

    public generateSdp() {
        var that = this;
        return new Promise((resolve, reject) => {
            var media = { audio: true, video: true };
            var audioSupport = media.audio 
            var height = 480;
            var width = 640;
            var videoSupport = {
                'height': { 'ideal': height },
                'width': { 'ideal': width }
            };
            var gumConstraints = {
                    audio: audioSupport,
                    video: videoSupport 
                };
            navigator.mediaDevices.getUserMedia(gumConstraints)
                .then(function (stream) { that.streamsDone(media, stream).then(resolve, reject); })
                .catch(function (error) { reject(error); });
        })
    }

    setOnLocalStream(localStream: (stream: MediaStream) => void){
        this._localStream = localStream;
    }

    sendTrickleCandidate: (candidate: any) => void;
    pc: any;
    stun = "stun:89.249.28.54:3478";
    iceDone = false;
    trickle = true;


    _localStream: (stream: MediaStream) => void;
    myStream:any;

    private streamsDone(media: any, stream: any) {
        return new Promise((resolve, reject) => {
            var that = this;
            if (stream) {
                console.debug("  -- Audio tracks:", stream.getAudioTracks());
                console.debug("  -- Video tracks:", stream.getVideoTracks());
            }
            // We're now capturing the new stream: check if we're updating or if it's a new thing
            this.myStream = stream;
            var addTracks = true;
            // If we still need to create a PeerConnection, let's do that
            var iceTransportPolicy ;
            var bundlePolicy ;
            var iceServers = [{ urls: this.stun}];
            var pc_config = { "iceServers": iceServers, "iceTransportPolicy": iceTransportPolicy, "bundlePolicy": bundlePolicy };
            var pc_constraints: any = {
                "optional": [{ "DtlsSrtpKeyAgreement": true }]
            };
            
            this.pc = new RTCPeerConnection(pc_config);
            console.log("Preparing local SDP and gathering candidates (trickle=" + this.trickle + ")");
            this.pc.oniceconnectionstatechange = function () {
                if (that.pc)
                    console.log("Ice state changed", that.pc.iceConnectionState);
            };
            this.pc.onicecandidate = function (event: any) {
                if (event.candidate == null) {
                    that.iceDone = true;
                    if (that.trickle === true) {
                        // End of candidates
                        that.sendTrickleCandidate({ "completed": true });
                    }
                    // } else {
                    //     // No trickle, time to send the complete SDP (including all candidates)
                    //     that.sendSDP(callbacks);
                    // } //kek turn off trickle and send complete sdp
                } else {
                    var candidate = {
                        "candidate": event.candidate.candidate,
                        "sdpMid": event.candidate.sdpMid,
                        "sdpMLineIndex": event.candidate.sdpMLineIndex,
                        "completed": false
                    };
                    if (that.trickle === true) {
                        // Send candidate
                        that.sendTrickleCandidate(candidate);
                    }
                }
            };
            if (addTracks && stream !== null && stream !== undefined) {
                console.log('Adding local stream');
                stream.getTracks().forEach(function (track: any) {
                    console.log('Adding local track:', track);
                    that.pc.addTrack(track, stream);
                });
            }
            
            // If there's a new local stream, let's notify the application
            if (that.myStream) {
                this._localStream(that.myStream);
            }
            // Create offer/answer now
            that.createOffer(media).then(resolve, reject);
            
        })
    }

    private createOffer(media: any) {
        var that = this;
        return new Promise((resolve, reject) => {
            console.log("Creating offer (iceDone=" + that.iceDone + ")");
            var mediaConstraints: any = {};
            mediaConstraints["offerToReceiveAudio"] = false;
            mediaConstraints["offerToReceiveVideo"] = false;
            that.pc.createOffer(
                function (offer: any) {
                    console.debug(offer);
                    console.log("Setting local description");
                    that.pc.setLocalDescription(offer);
                    // if (!config.iceDone && !config.trickle) {
                    //     // Don't do anything until we have all candidates
                    //     console.log("Waiting for all candidates...");
                    //     return;
                    // } kek: turn of trickles - do this
                    console.log("Offer ready");
                    var jsep = {
                        "type": offer.type,
                        "sdp": offer.sdp
                    };
                    resolve(jsep);
                }, reject, mediaConstraints);
        })
    }

    public handleAnswer(jsep: any) {
        var that = this;
        if (jsep !== undefined && jsep !== null) {
            if (that.pc === null) {
                console.warn("Wait, no PeerConnection?? if this is an answer, use createAnswer and not handleRehandleRemoteJsep");
                return;
            }
            that.pc.setRemoteDescription(
                new RTCSessionDescription(jsep),
                function () {
                    console.log("Remote description accepted!");
                }, (er: any) => { console.log(er) });
        } else {
            (er: any) => { console.log(er) };
        }
    }
}