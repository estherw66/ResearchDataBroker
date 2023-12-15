import React, {useState, useEffect} from "react";
import axios from "axios";

const IndexDataset = () => {
    const [datasetUrl, setDatasetUrl] = useState("")
    const [items, setItems] = useState([])

    useEffect(() => {
        
    }, []);
    
    useEffect(() => {
        console.log(items)
    }, [items]);
    
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
    
    const handleDatasetUrlChange = (e) => {
        setDatasetUrl(e.target.value)
    }
    
    const sendRequest = () => {
        let requestData = {
            'DatasetUrl': datasetUrl
        }
        
        console.log(requestData)
        console.log(datasetUrl)
        
        axios.post('api/index', requestData)
            .then(res =>{
                setItems(res.data.itemDTOs)
            })
            .catch(err => {
                console.log(err)
            })
    }

    return (
        <main>
            <form onSubmit={handleSubmit}>
                <div>
                    <label className='col-form-label-lg'>Dataset url:</label>
                    <input id='datasetUrl' value={datasetUrl} onChange={handleDatasetUrlChange} required />
                </div>
                <div>
                    <button className='btn-danger'>Index dataset</button>
                </div>
            </form>
            {items.length > 0 ? (
                <>
                    <table className='table'>
                        <thead>
                        <tr>
                            <th>Item Name</th>
                            <th>Amount indexed</th>
                        </tr>
                        </thead>
                        <tbody>
                        {items?.map(
                            item =>
                                <tr key={item.id}>
                                    <td>{item.name}</td>
                                    <td>{item.fileIds.length}</td>
                                </tr>
                        )}
                        </tbody>
                    </table>
                </>
            ) : (
                <></>
            )}
        </main>
    )
}

export default IndexDataset