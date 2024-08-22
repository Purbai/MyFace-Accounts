import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    headerDetails: (authString: string) =>{},
    logIn: () => {},
    logOut: () => {},
    //header:"",
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(true);
    const [header,setHeader] = useState("");
    
    function logIn() {
        setLoggedIn(true);
    }
    
    function logOut() {
        setLoggedIn(false);
    }

    function headerDetails(authString:string) {
        setHeader(authString);
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        headerDetails : headerDetails,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}