import * as React from "react"
interface IState{
    groupGuid: string
}

interface IProps{
    onCallInitiated: (groupGuid: string, onLocalStream: (stream :MediaStream)=> void, onRemoteStream: (stream: MediaStream)=> void) => void; 
}

export default class InitiateVideocallInvoker extends React.Component<IProps, IState>{
    constructor(props: IProps){
        super(props);
        this.state = { 
            groupGuid: "eb0d42c7-5aaa-4751-b949-fc5c3f5769b2"
        }
        this.onLocalStream = this.onLocalStream.bind(this);
        this.onRemoteStream = this.onRemoteStream.bind(this);
        this.onCallInit = this.onCallInit.bind(this);
    }

    onLocalStream(stream: MediaStream){
        console.log("[KEK] local: ", stream.active, " ", stream);
        (this.refs.localVideo as HTMLVideoElement).srcObject = stream;
    }

    onRemoteStream(stream: MediaStream){
        console.log("[KEK] remote: ", stream.active, " ", stream);
        (this.refs.remoteVideo as HTMLVideoElement).srcObject = stream;
    }

    onCallInit(){
        this.props.onCallInitiated(this.state.groupGuid, this.onLocalStream, this.onRemoteStream);
    }

    render(){
        return <div style={{display: "flex", flexDirection: "column", alignItems:"start", borderBottomColor: "gray", borderBottomWidth:"2px", borderBottomStyle: "solid", marginTop:"16px"}}>
            <span>Init videocall, join 1234 as publisher</span>
            <div style={{display: "flex", flexDirection: "row", marginTop: "8px"}}>
                <span>groupGuid: string</span>
                <input 
                    value={this.state.groupGuid}
                    type="text"
                    onChange={(ev: React.ChangeEvent<HTMLInputElement>)=>this.setState({groupGuid: (ev.target as HTMLInputElement).value})}/>
            </div>
            <div style={{display: "flex", flexDirection:"row", height: 480, marginTop: "8px"}}>
                <video ref="localVideo" autoPlay playsInline muted/>
                <video ref="remoteVideo" autoPlay playsInline controls/>
            </div>

            <button style={{marginTop: "8px"}} onClick={()=> this.onCallInit()}>Init call</button>
        </div>
    }
}
