var adapter = require("webrtc-adapter")
export default class WebrtcStuff{

    constructor(onIceCandidate: (candidate: any) => void){
        this.sendTrickleCandidate = onIceCandidate;
    }

    

    trickle = true;
    public generateSdp(jsep?: any) {
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
                .then(function (stream) { that.streamsDone(media, stream, jsep).then(resolve, reject); })
                .catch(function (error) { reject(error); });
        })
    }

    setOnRemoteStream(remoteStream: (stream: MediaStream) => void){
        this._remoteStream = remoteStream
    }
    setOnLocalStream(localStream: (stream: MediaStream) => void){
        this._localStream = localStream;
    }

    sendTrickleCandidate: (candidate: any) => void;
    pc: RTCPeerConnection;
    stun = "stun:89.249.28.54:3478";
    iceDone = false;

    _remoteStream: (stream: MediaStream) => void;
    remoteStream: MediaStream;
    _localStream: (stream: MediaStream) => void;
    myStream:any;

    private streamsDone(media: any, stream: any, jsep: any) {
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
            // var pc_constraints: any = {
            //     "optional": [{ "DtlsSrtpKeyAgreement": true }]
            // };
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
            that.pc.ontrack = function (event: any) {
                console.log("Handling Remote Track");
                console.debug(event);
                if (!event.streams)
                    return;
                that.remoteStream = event.streams[0];
                that._remoteStream(that.remoteStream);
                if (event.track && !event.track.onended) {
                    console.log("Adding onended callback to track:", event.track);
                    event.track.onended = function (ev: any) {
                        console.log("Remote track removed:", ev);
                        if (that.remoteStream) {
                            that.remoteStream.removeTrack(ev.target);
                            that._remoteStream(that.remoteStream);
                        }
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
            if (jsep === null || jsep === undefined) {
                that.createOffer(media).then(resolve, reject);
            } else {
                console.log("Setting remote description")
                var description1 = new RTCSessionDescription(jsep)
                console.log("Session description generared hmmm not here")
                that.pc.setRemoteDescription(
                    description1,
                    function () {
                        console.log("Remote description accepted!");
                        that.createAnswer(media).then(resolve, reject);
                    }, (er: any)=> {throw er});
            }
        })
    }

    private createOffer(media: any) {
        var that = this;
        return new Promise((resolve, reject) => {
            console.log("Creating offer (iceDone=" + that.iceDone + ")");
            var mediaConstraints: any = {};
            mediaConstraints["offerToReceiveAudio"] = true;
            mediaConstraints["offerToReceiveVideo"] = true;
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

    private createAnswer(media: any) {
        var that = this;
        return new Promise((resolve, reject) => {
            console.log("Creating answer (iceDone=" + that.iceDone + ")");
            var mediaConstraints: any = null;
            mediaConstraints = {
                mandatory: {
                    OfferToReceiveAudio: true,
                    OfferToReceiveVideo: true
                }
            };
            that.pc.createAnswer(
                function (answer: any) {
                    console.debug(answer);
                    console.log("Setting local description");
                    that.pc.setLocalDescription(answer);
                    var jsep = {
                        "type": answer.type,
                        "sdp": answer.sdp
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