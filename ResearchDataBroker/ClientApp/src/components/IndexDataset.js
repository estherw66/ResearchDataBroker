import React, {useState, useEffect} from "react";
import axios from "axios";

const IndexDataset = () => {
    const [datasetUrl, setDatasetUrl] = useState("")
    const [items, setItems] = useState([])

    useEffect(() => {
        
    }, []);
    
    const validateInput = () => {
        let errorMsg = ''
        
        if (datasetUrl === null){
            errorMsg += 'Please enter a url.'
        }
        
        return errorMsg;
    }
    
    const handleSubmit = (e) => {
        e.preventDefault()
        
        let errorMsg = validateInput();
        
        if (errorMsg !== ''){
            alert(errorMsg)
        } else {
            sendRequest()
        }
    }
    
    const sendRequest = () => {
        const url = '/api/index'
        
        let requestData = {
            'DatasetUrl': datasetUrl
        }
        
        console.log(requestData)
        console.log(url)
        
        axios.post(datasetUrl, requestData)
            .then(res =>{
                setItems(res.data.itemDTOs)
                console.log(items)
            })
            .catch(err => {
                console.log(err)
            })
    }

    return (
        <main>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Dataset url:</label>
                    <input id='url' value={datasetUrl} onChange={(e) => setDatasetUrl(e.target.value)} required />
                </div>
                <div>
                    <button className='btn-danger'>Index dataset</button>
                </div>
            </form>
        </main>
    )
}

export default IndexDataset

// <input id='check-out' type={'date'} value={checkOutDate} min={defaultCheckOut} onChange={(e) => setCheckOutDate(e.target.value)} required />
