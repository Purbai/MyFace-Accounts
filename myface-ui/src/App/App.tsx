import React, {ReactElement, useContext, useState} from 'react';
import './App.scss';
import {BrowserRouter as Router, Switch, Route} from "react-router-dom";
import {Feed} from "../Pages/Feed/Feed";
import {Users} from "../Pages/Users/Users";
import {NotFound} from "../Pages/NotFound/NotFound";
import {Login} from "../Pages/Login/Login";
import {LoginContext, LoginManager} from "../Components/LoginManager/LoginManager";
import {Profile} from "../Pages/Profile/Profile";
import {CreatePost} from "../Pages/CreatePost/CreatePost";

interface LoginProps {
    username: string;
    setUsername: (username: string) => void;
    password: string;
    setPassword: (password: string) => void;
}

function Routes(props: LoginProps ): ReactElement {
    const loginContext = useContext(LoginContext);
    
    if (!loginContext.isLoggedIn) {
        return <Login username={props.username} setUsername={props.setUsername} setPassword={props.setPassword} password={props.password} />
    }


    
    return (
        <Switch>
            <Route exact path="/" component={Feed}/>
            <Route exact path="/users" component={Users}/>
            <Route exact path="/users/:id" component={Profile}/>
            <Route exact path="/new-post" component={CreatePost}/>
            <Route exact path="/Login" component={Login }/>
            <Route path="" component={NotFound}/>
        </Switch>
    );
}

export default function App(): ReactElement {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    return (
        <Router>
            <LoginManager>
                <Routes username={username} setUsername={setUsername} setPassword={setPassword} password={password}/>
            </LoginManager>
        </Router>
    );
}
