import { resolve } from "url";

var adapter = require("webrtc-adapter")

export default class RemoteWebrtcStuff{

    constructor(onIceCandidate: (candidate: any) => void){
        this.sendTrickleCandidate = onIceCandidate;
    }

    private onRemoteStream : (stream: MediaStream) => void;
    public setOnRemoteStream(onRemoteStream: (stream: MediaStream) => void){
        this.onRemoteStream = onRemoteStream;
    }
    private remoteStream : MediaStream;
    sendTrickleCandidate: (candidate: any) => void;
    pc: any;
    stun = "stun:89.249.28.54:3478";
    iceDone = false;
    trickle = true;

    public generateSdp(jsep: any) {
        var that = this;
        return new Promise((resolve, reject) => {
            var media = { audioSend: false, videoSend: false, update: false };
            that.streamsDone(media, jsep).then(resolve, reject);
        })
    }

    private streamsDone(media: any, jsep: any){
        var that = this;
        return new Promise((resolve, reject)=>{
            var addTracks = true;
            var pc_config = {"iceServers": [{"urls": this.stun}]};

			//~ var pc_constraints = {'mandatory': {'MozDontOfferDataChannel':true}};
			var pc_constraints = {
				"optional": [{"DtlsSrtpKeyAgreement": true}]
            };
            that.pc = new RTCPeerConnection(pc_config);

			that.pc.onicecandidate = function(event:any) {
				if (event.candidate == null) {
					that.iceDone = true;
                    that.sendTrickleCandidate({"completed": true});
				} else {
					// JSON.stringify doesn't work on some WebRTC objects anymore
					// See https://code.google.com/p/chromium/issues/detail?id=467366
					var candidate = {
						"candidate": event.candidate.candidate,
						"sdpMid": event.candidate.sdpMid,
						"sdpMLineIndex": event.candidate.sdpMLineIndex
					};
					that.sendTrickleCandidate(candidate);
				}
            };
            that.pc.ontrack = function(event: any) {
				if(!event.streams)
					return;
				that.remoteStream = event.streams[0];
				that.onRemoteStream(event.streams[0]);
				if(event.track && !event.track.onended) {
					event.track.onended = function(ev: any) {
						if(that.remoteStream) {
                            console.error("U should delete all shit here")
                        }
					}
                }
            };
            that.pc.setRemoteDescription(new RTCSessionDescription(jsep))
                .then(()=>{
                    that.createAnswer().then(resolve, reject)
                })
                .catch(reject)
        })
    }
    createAnswer(){
        var that = this;
        return new Promise((resolve, reject)=>{
            var mediaConstraints = {mandatory: {OfferToReceiveAudio: true, OfferToReceiveVideo: true}}
            that.pc.createAnswer(
                function (answer: any) {
                    that.pc.setLocalDescription(answer);
                    var jsep = {
                        "type": answer.type,
                        "sdp": answer.sdp
                    };
                    resolve(jsep);
                }, reject, mediaConstraints);
        })
    }
}
