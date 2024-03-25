import React, { useState,useEffect  } from 'react'
import { LineChart, Line, XAxis, YAxis, ResponsiveContainer } from 'recharts';
import {SmartHome} from './SmartHome.js'
import ReactDOM from 'react-dom/client';
import temp from './img/temp.svg'
import sun from './img/sun.svg'
import power from './img/power.svg'
import ajust from './img/ajust.svg'


export function TrackHome({
    token
  }) {


    const [dataChart, setDataChart] = useState({device1:[], device2:[], device3:[], device4:[]});

    useEffect(() => {

      async function setData(){
        const requestOptions = {
          method: 'POST',
          headers: { 'Accept':'*/*', 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' },
          body: JSON.stringify({Token: token })
        };
        const response = await fetch('https://scapiweboscket.azurewebsites.net/logs/Devices', requestOptions)
        const respJson = await response.json()
        console.log(respJson)

        setDataChart(respJson)
      }

      setData()
       
    }, []);

    function returnBack(){
      const root = ReactDOM.createRoot(document.getElementById("root"));
      root.render(<SmartHome token={token}></SmartHome>);
    }



    console.log(dataChart)

    return (
      //   <LineChart 
      //      width={800} 
      //      height={400} 
      //      data={dataChart.device1}>
      //     <XAxis dataKey="timestamp" tickFormatter={(date) => new Date(date).toLocaleTimeString('es-ES')}/>
      //     <YAxis />
      //     <Line type="monotone"  dataKey="light" stroke="#84B8D8" />
      //   <Line type="monotone"  dataKey="temp" stroke="#D884D7" />
      // </LineChart>
      <div className="container-smart">
         <div id="c1">
          <div className="RoomTitle">Living Room</div>
          <div style={{ width: '100%', height: 300 , paddingRight:35}}>
          <ResponsiveContainer>
          <LineChart 

data={dataChart.device1}>
<XAxis dataKey="timestamp" tickFormatter={(date) => new Date(date).toLocaleTimeString('es-ES')} interval={40}/>
<YAxis />
<Line type="monotone"  dataKey="light" stroke="#84B8D8" strokeWidth={1} dot={false} />
<Line type="monotone"  dataKey="temp" stroke="#D884D7" strokeWidth={1} dot={false} />
</LineChart>
</ResponsiveContainer>
          </div>
            

         </div>
         <div id="c2">
          <div className="RoomTitle">Bathroom</div>
          <div style={{ width: '100%', height: 300 , paddingRight:35}}>
          <ResponsiveContainer>
          <LineChart 

data={dataChart.device2}>
<XAxis dataKey="timestamp" tickFormatter={(date) => new Date(date).toLocaleTimeString('es-ES')} interval={40}/>
<YAxis />
<Line type="monotone"  dataKey="light" stroke="#84B8D8" strokeWidth={1} dot={false} />
<Line type="monotone"  dataKey="temp" stroke="#D884D7" strokeWidth={1} dot={false} />
</LineChart>
</ResponsiveContainer>
          </div>
         </div>
         <div id="c3">
          <div className="RoomTitle">Bedroom</div>
          <div style={{ width: '100%', height: 300 , paddingRight:35}}>
          <ResponsiveContainer>
          <LineChart 

data={dataChart.device3}>
<XAxis dataKey="timestamp" tickFormatter={(date) => new Date(date).toLocaleTimeString('es-ES')} interval={40}/>
<YAxis />
<Line type="monotone"  dataKey="light" stroke="#84B8D8" strokeWidth={1} dot={false} />
<Line type="monotone"  dataKey="temp" stroke="#D884D7" strokeWidth={1} dot={false} />
</LineChart>
</ResponsiveContainer>
          </div>
         </div>
         <div id="c4">
          <div className="RoomTitle">Kitchen</div>
          <div style={{ width: '100%', height: 300 , paddingRight:35}}>
          <ResponsiveContainer>
          <LineChart 

data={dataChart.device4}>
<XAxis dataKey="timestamp" tickFormatter={(date) => new Date(date).toLocaleTimeString('es-ES')} interval={40}/>
<YAxis />
<Line type="monotone"  dataKey="light" stroke="#84B8D8" strokeWidth={1} dot={false}  />
<Line type="monotone"  dataKey="temp" stroke="#D884D7"  strokeWidth={1} dot={false} />
</LineChart>
</ResponsiveContainer>
          </div>
         </div>

         <div id="c0-little">
         <img className=" ImgSystem ImgTitle-little" src = {ajust} onClick={returnBack}/>
         </div>
         </div> 
    );
  
}