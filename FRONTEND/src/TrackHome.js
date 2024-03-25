import React, { useState,useEffect  } from 'react'
import { LineChart, Line } from 'recharts';


export function TrackHome({
    token
  }) {
    const data2 = [{name: 'Page A', uv: 400}, 
    {name: 'Page B', uv: 500}];

    const [dataChart, setDataChart] = useState([{name: 'Page A', temp: 400}, {name: 'Page B', temp: 500}]);

    useEffect(async () => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept':'*/*', 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': '*' },
            body: JSON.stringify({Token: token })
          };
          const response = await fetch('https://scapiweboscket.azurewebsites.net/logs/get', requestOptions)

          setDataChart(await response.json())
    }, []);




    console.log(dataChart)

    return (
        <LineChart width={400} height={400} data={dataChart}>
        <Line type="monotone" dataKey="temp" stroke="#8884d8" />
      </LineChart>
    );
  
}