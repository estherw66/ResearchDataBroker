import React, {useEffect, useState} from "react";
import axios from "axios";

const IndexDatasetComponent = () => {
    const [datasetUrl, setDatasetUrl] = useState('')
    const [items, setItems] = useState([])

    useEffect(() => {
        
    }, []);
    
    const handleDatasetUrlChange = (e) => {
        setDatasetUrl(e.target.value)
    }
    
    const handleSubmit = (e) => {
        e.preventDefault()
        
        sendRequest()
    }
    
    const sendRequest = () => {
        let requestData = {
            'DatasetUrl': datasetUrl
        }
        
        axios.post('api/index', requestData)
            .then(res => {
                setItems(res.data.itemDTOs)
            })
            .catch(err => {
                console.log(err)
            })
    }
    
    
    return (
        <>
            <div className={'search-container'}>
                <div className={'search-box'}>
                    <h1 className={'title'}>Dataset toevoegen:</h1>
                    <form onSubmit={handleSubmit}>
                        <input className={'search-bar'} type={'text'} placeholder={'url'} value={datasetUrl} onChange={handleDatasetUrlChange} required />
                        <button className={'search-btn'}>Toevoegen</button>
                    </form>
                </div>
                <hr/>
            </div>
            <div>
                {items.length > 0 ? (
                    <>
                        <table className={'table'}>
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
            </div>
        </>
    )
}

export default IndexDatasetComponent