import React, { FormEvent, useContext, useState } from 'react';
import { Page } from "../Page/Page";
import { LoginContext } from "../../Components/LoginManager/LoginManager";
import "./Login.scss";
import { fetchLogin } from '../../Api/apiClient';

interface LoginProps {
    username: string;
    setUsername: (username: string) => void;
    password: string;
    setPassword: (password: string) => void;
}
export function Login(props: LoginProps): JSX.Element {
    const loginContext = useContext(LoginContext);

    // const [username, setUsername] = useState("");
    // const [password, setPassword] = useState("");

    function tryLogin(event: FormEvent) {
        event.preventDefault();
        //console.log("testing - inside trylogin ......");
        //console.log(`username = ${props.username} , password = ${props.password}, setusername = ${props.setUsername}, setpassword = ${props.setPassword}`);
        // API to fetch our user
        const loginResult = fetchLogin(props.username, props.password)
        //console.log(`checking the results after the fetchloginapi call .... ${loginResult}`)
        if (!loginResult == null) {
            // valid user
            //loginContext.headerDetails(`Authorization: Basic ${btoa(props.username + ":" + props.password)}`)
            console.log(`headerdetail .... ${loginContext.headerDetails}`);
            loginContext.logIn();
        }
        else {
            //loginContext.headerDetails(`Authorization: Basic :`)
            // else the user is not valid
            loginContext.logOut();
        }
    }

    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            <form className="login-form" onSubmit={tryLogin}>
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={props.username} onChange={event => props.setUsername(event.target.value)} />
                </label>

                <label className="form-label">
                    Password
                    {/* <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)}/> */}
                    <input className="form-input" type={"password"} value={props.password} onChange={event => props.setPassword(event.target.value)} />
                </label>

                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}