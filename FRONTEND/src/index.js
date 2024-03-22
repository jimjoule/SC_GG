import React from 'react'
import ReactDOM from 'react-dom/client';
import './index.css';
import {SmartHome} from './SmartHome.js'
import { useState } from "react";
import { ThreeCircles } from 'react-loader-spinner';
import * as signalR from '@microsoft/signalr';

  function App  ({
    submit,
    userText,
    onUserTextChange,
    pswText,
    onPswTextChange,
    showLog,
    error
  }) {
  
       
      return(

        <div className="body-custom">

          <div className="container">
            <div className="form-box">

              <input 
                id="usr" 
                type="text" 
                className="form-control login-input" 
                placeholder="Username" 
                value={userText} 
                onChange={(e) => onUserTextChange(e.target.value)}/>
            
            
              <input 
              id="psw" 
              type="password" 
              className="form-control login-input" 
              placeholder="Password" 
              value={pswText} 
              onChange={(e) => onPswTextChange(e.target.value)}/>
          
                
              <div id="btn-remember" >
                <div id="ErrorLogin">{error}</div>
                <div></div>
                <ThreeCircles
  visible={showLog}
  height="20"
  width="20"
  color="#1B4166"
  ariaLabel="tail-spin-loading"
  radius="6"
  wrapperStyle={{}}
  wrapperClass=""
  />
              </div>

          
                <button id="btn-login" type="button" className="btn btn-secondary btn-block" onClick={submit}>LOGIN</button>
          
            </div>
          </div> 
        </div>
        
     )
    
  }


  export default function Counter() {
    const [error, setError] = useState('');
    const [user, setUser] = useState('');
    const [password, setPassword] = useState('');
    const [showLogin, setShowLogin] = useState(false);

    async function login(){
    console.log(user)
    setShowLogin(true)
      // Simple POST request with a JSON body using fetch
      const requestOptions = {
        method: 'POST',
        headers: { 'Accept':'*/*', 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' },
        body: JSON.stringify({User: user, Password: password })
    };
    const response = await fetch('https://scapiweboscket.azurewebsites.net/auth/login', requestOptions)
    const data = await response.json();
    console.log(data)
    if(response.status == 200){
      root.render(<SmartHome token={data.token}></SmartHome>);
      setShowLogin(false)
      
    }
    else{
      setShowLogin(false)
      setError('Invalid credentials')
    }
      };
     
    return (
      <App
        submit={login}
        onUserTextChange={setUser} 
        userText={user}
        pswText={password}
        onPswTextChange={setPassword}
        showLog={showLogin}
        error={error}
      ></App>
    );
  }

  // ========================================
  
  const root = ReactDOM.createRoot(document.getElementById("root"));
  root.render(<Counter />);