import {Configuration} from "@azure/msal-browser";

export const msalConfig : Configuration = {
    auth: {
        clientId: "1c867064-aceb-4878-93d4-3683929336a2",
        authority: "https://login.microsoftonline.com/2e521f39-94b6-4f52-8600-7340a47238c3", // This is a URL (e.g. https://login.microsoftonline.com/{your tenant ID})
        redirectUri: "http://localhost:5173/",
    },
    cache: {
        cacheLocation: "sessionStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    }
};

// Add scopes here for ID token to be used at Microsoft identity platform endpoints.
export const loginRequest = {
    scopes: ["api://acf28637-a463-4afd-9592-64914f52b13c/Access.as.User"]
};

// Add the endpoints here for Microsoft Graph API services you'd like to use.
export const graphConfig = {
    graphMeEndpoint: "https://graph.microsoft.com/v1.0/me"
};