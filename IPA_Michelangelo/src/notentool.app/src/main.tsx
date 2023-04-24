import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import './index.css'
import { MsalProvider } from "@azure/msal-react";
import { msalConfig } from "./AuthConfig";
import {PublicClientApplication} from "@azure/msal-browser";

const msalInstance = new PublicClientApplication(msalConfig);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <MsalProvider instance={msalInstance}>
            <App />
        </MsalProvider>
    </React.StrictMode>
)
