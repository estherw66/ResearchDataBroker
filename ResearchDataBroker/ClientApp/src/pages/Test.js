import React, { useState, useEffect } from 'react';
import axios from 'axios'

const Test = () => {
    const [message, setMessage] = useState("")

    useEffect(() => {
        axios.get('api/test')
            .then(response => {
                setMessage(response.data['message']);
                console.log(response.data);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });
    }, []);

    return (
        <main>
            <h1>Testing the api</h1>
            {
                (message !== "") ? <h3>{message}</h3> : <h3>Loading...</h3>
            }
        </main>
    )
}

export default Test