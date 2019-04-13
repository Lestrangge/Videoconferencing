import * as React from "react";
import * as ReactDOM from "react-dom";
import { ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import Main from "./componets/main/main";
ReactDOM.render(
    <div>
        <Main/>
        <ToastContainer autoClose={2000} newestOnTop={false} closeOnClick={false} pauseOnHover={true} hideProgressBar/>
    </div>,
    document.getElementById("app")
);