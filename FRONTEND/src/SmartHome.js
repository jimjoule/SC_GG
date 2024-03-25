import React, { useState,useEffect  } from 'react'
import ReactDOM from 'react-dom/client';
import './SmartHome.css'
import temp from './img/temp.svg'
import sun from './img/sun.svg'
import power from './img/power.svg'
import ajust from './img/ajust.svg'
import * as signalR from '@microsoft/signalr';
import Connector from './signalr-conn.ts'
import {TrackHome} from './TrackHome.js'




  
 function SmartHomeView({
    temp1,
    light1,
    temp2,
    light2,
    temp3,
    light3,
    temp4,
    light4,
    token,
    freq1,
    setFreq1,
    freq2,
    setFreq2,
    freq3,
    setFreq3,
    freq4,
    setFreq4,
    send,
    stateD1,
    stateD2,
    stateD3,
    stateD4
  }) {

    function increment () {
      setFreq1(freq1 + 1)
      send("1", freq1)
    }
    function decrement () {
      if(freq1 > 1){
        setFreq1(freq1 - 1)
        send("1", freq1)
      }
    }
    function increment2 () {
      setFreq2(freq2 + 1)
      send("2", freq2)
    }
    function decrement2 () {
      if(freq2 > 1){
        setFreq2(freq2 - 1)
        send("2", freq2)
      }
    }
    function increment3 () {
      setFreq3(freq3 + 1)
      send("3", freq3)
    }
    function decrement3 () {
      if(freq3 > 1){
        setFreq3(freq3 - 1)
        send("3", freq3)
      }
    }
    function increment4 () {
      setFreq4(freq4 + 1)
      send("4", freq4)
    }
    function decrement4 () {
      if(freq4 > 1){
        setFreq4(freq4 - 1)
        send("4", freq4)
      }
    }

    const [tempHome, setTempHome] = useState(0);
    const [lightHome, setLightHome] = useState(0);
    const [txtD1, setTextD1] = useState('');
    const [txtD2, setTextD2] = useState('');
    const [txtD3, setTextD3] = useState('');
    const [txtD4, setTextD4] = useState('');

    function Reload(){
      window.location.reload()
    }

    async function Logs({
    }
    ) {
      // const requestOptions = {
      //   method: 'POST',
      //   headers: { 'Accept':'*/*', 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' },
      //   body: JSON.stringify({Token: token })
      // };
      // const response = await fetch('https://scapiweboscket.azurewebsites.net/logs/get', requestOptions)
      // const data = await response.json();
      // var str = JSON.stringify(data, null, 2); // spacing level = 2
       const root = ReactDOM.createRoot(document.getElementById("root"));
      // root.render(<div>{str}</div>);
      root.render(<TrackHome token={token}></TrackHome>);
    
    }

    useEffect(() => {
      setLightHome(((parseFloat(light1)+parseFloat(light2)+parseFloat(light3)+parseFloat(light4))/4).toFixed(1))
      setTempHome(((parseFloat(temp1)+parseFloat(temp2)+parseFloat(temp3)+parseFloat(temp4))/4).toFixed(1))
    }, [ temp1,
      light1,
      temp2,
      light2,
      temp3,
      light3,
      temp4,
      light4,]);

      useEffect(() => {
          if(stateD1){
            document.getElementById("c1").classList.remove('inactive');
            setTextD1('')
          }
          else{
            document.getElementById("c1").classList.add('inactive');
            setTextD1('Disconnected')
          }
          if(stateD2){
            document.getElementById("c2").classList.remove('inactive');
            setTextD2('')
          }
          else{
            document.getElementById("c2").classList.add('inactive');
            setTextD2('Disconnected')
          }
          if(stateD3){
            document.getElementById("c3").classList.remove('inactive');
            setTextD3('')
          }
          else{
            document.getElementById("c3").classList.add('inactive');
            setTextD3('Disconnected')
          }
          if(stateD4){
            document.getElementById("c4").classList.remove('inactive');
            setTextD4('')
          }
          else{
            document.getElementById("c4").classList.add('inactive');
            setTextD4('Disconnected')
          }
      }, [ stateD1, stateD2, stateD3, stateD4]);

      
      return(
        <div className="container-smart">
         <div id="c1">
          <div className="RoomTitle">Living Room</div>
          <div className='Content-Data'><img className="ImgTitle ImgTemp" src = {temp}/><div className='Value-Data' >{temp1}</div></div>
          <div className='Content-Data'><img className="ImgTitle ImgSun" src = {sun}/><div className='Value-Data'>{light1}</div></div>
         </div>
         <div id="c2">
          <div className="RoomTitle">Bathroom</div>
          <div className='Content-Data'><img className="ImgTitle ImgTemp" src = {temp}/><div className='Value-Data'>{temp2}</div></div>
          <div className='Content-Data'><img className="ImgTitle ImgSun" src = {sun}/><div className='Value-Data'>{light2}</div></div>
         </div>
         <div id="c3">
          <div className="RoomTitle">Bedroom</div>
          <div className='Content-Data'><img className="ImgTitle ImgTemp" src = {temp}/><div className='Value-Data'>{temp3}</div></div>
          <div className='Content-Data'><img className="ImgTitle ImgSun" src = {sun}/><div className='Value-Data'>{light3}</div></div>
         </div>
         <div id="c4">
          <div className="RoomTitle">Kitchen</div>
          <div className='Content-Data'><img className="ImgTitle ImgTemp" src = {temp}/><div className='Value-Data'>{temp4}</div></div>
          <div className='Content-Data'><img className="ImgTitle ImgSun" src = {sun}/><div className='Value-Data'>{light4}</div></div>
         </div>

         <div id="c0">
         <img className=" ImgSystem ImgTitle" src = {ajust} onClick={Logs}/>
            <div></div>
            <div className="RoomTitleMain">HOME</div>
            <div className='Content-Data'><img className="ImgTitle ImgTemp" src = {temp}/><div className='Value-Data HomeData'>{tempHome}</div></div>
            <div className='Content-Data'><img className="ImgTitle ImgSun" src = {sun}/><div className='Value-Data HomeData'>{lightHome}</div></div>
            <div></div>
            <img className=" ImgSystem ImgTitle" src = {power} onClick={Reload}/>
         </div>
         <div className="con-1">
            <div className="D1">{txtD1}</div>
          </div>
          <div className="con-2">
            <div className="D1">{txtD2}</div>
          </div>
          <div className="con-3">
            <div className="D3">{txtD3}</div>
          </div>
          <div className="con-4">
            <div className="D3">{txtD4}</div>
          </div>
         <div className="btns-1">
            <button className="plus" onClick={increment}>+</button>
            <div className="frequences">{freq1}</div>
            <button className="plus" onClick={decrement}>-</button>
          </div>
          <div className="btns-2">
            <button className="plus" onClick={increment2}>+</button>
            <div className="frequences">{freq2}</div>
            <button className="plus" onClick={decrement2}>-</button>
          </div>
          <div className="btns-3">
            <button className="plus" onClick={increment3}>+</button>
            <div className="frequences">{freq3}</div>
            <button className="plus" onClick={decrement3}>-</button>
          </div>
          <div className="btns-4">
            <button className="plus" onClick={increment4}>+</button>
            <div className="frequences">{freq4}</div>
            <button className="plus" onClick={decrement4}>-</button>
          </div>
         </div> 
     )
    
  }



 



  export function SmartHome({
    token
  }) {

    async function ReciveData (locationData)  {
      const reg= JSON.parse( locationData );
    console.log(reg)
      switch(reg.deviceId){
        case '1':
          setTemp1(reg.temp.toFixed(1))
          setLight1(reg.light.toFixed(1))
          break
        case '2':
          setTemp2(reg.temp.toFixed(1))
          setLight2(reg.light.toFixed(1))
          break 
        case '3':
          setTemp3(reg.temp.toFixed(1))
          setLight3(reg.light.toFixed(1))
          break
        case '4':
          setTemp4(reg.temp.toFixed(1))
          setLight4(reg.light.toFixed(1))
          break
      }
  }
  async function ReciveHeartBeat (locationData)  {
    //const reg= JSON.parse( locationData );
  console.log(locationData)
  if(locationData.includes('0')){
    if(locationData.includes('1')){
      setstateD1(true)
    }
    else{
      setstateD1(false)
    }
    if(locationData.includes('2')){
      setstateD2(true)
    }
    else{
      setstateD2(false)
    }
    if(locationData.includes('3')){
      setstateD3(true)
    }
    else{
      setstateD3(false)
    }
    if(locationData.includes('4')){
      setstateD4(true)
    }
    else{
      setstateD4(false)
    }
  }
  else{
    setstateD1(false)
    setstateD2(false)
    setstateD3(false)
    setstateD4(false)
  }
  
}


    const { newMessage, events } = Connector(token, ReciveData, ReciveHeartBeat);
    const [temp1, setTemp1] = useState(0);
    const [light1, setLight1] = useState(0);

    const [temp2, setTemp2] = useState(0);
    const [light2, setLight2] = useState(0);

    const [temp3, setTemp3] = useState(0);
    const [light3, setLight3] = useState(0);

    const [temp4, setTemp4] = useState(0);
    const [light4, setLight4] = useState(0);

    const [freq1, setFreq1] = useState(5);
    const [freq2, setFreq2] = useState(5);
    const [freq3, setFreq3] = useState(5);
    const [freq4, setFreq4] = useState(5);

    const [stateD1, setstateD1] = useState(true);
    const [stateD2, setstateD2] = useState(true);
    const [stateD3, setstateD3] = useState(true);
    const [stateD4, setstateD4] = useState(true);
    const [stateD0, setstateD0] = useState(true);

    const WS_URL = "https://scapiweboscket.azurewebsites.net/chat?token="+token;

function Send(device, number){
  newMessage(device, number)
}

    return (
   
      <SmartHomeView        
      temp1={temp1}
      light1={light1}
      temp2={temp2}
      light2={light2}
      temp3={temp3}
      light3={light3}
      temp4={temp4}
      light4={light4}
      token={token}
      freq1={freq1}
      setFreq1={setFreq1}
      freq2={freq2}
      setFreq2={setFreq2}
      freq3={freq3}
      setFreq3={setFreq3}
      freq4={freq4}
      setFreq4={setFreq4}
      send={Send}
      stateD1={stateD1}
      stateD2={stateD2}
      stateD3={stateD3}
      stateD4={stateD4}
      >
    </SmartHomeView>

    );
  
}
  
  // ========================================
  
