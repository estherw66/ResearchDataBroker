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

// {rooms.length > 0 ? (
//     <>
//         {rooms?.map((room) => (
//             room.totalAmountInHotel !== 0 ? (
//                 <>
//                     <RoomCard key={room.id} room={room} />
//                 </>
//             ) : (null)
//         ))}
//     </>
// ) : (
//     <p>Loading rooms...</p>
// )}

// <table className='table'>
//     <thead>
//     <tr>
//     <th>ID</th>
// <th>Room Type</th>
// <th>Capacity</th>
// <th>Price Per Night</th>
// <th>Total Amount</th>
// <th>Featured</th>
// <th>Image</th>
// <th></th>
// </tr>
// </thead>
// <tbody>
// {rooms?.map(
//     room =>
//         <tr key={room.id}>
//             <td>{room.id}</td>
//             <td>{room.roomType}</td>
//             <td>{room.capacity}</td>
//             <td>{room.pricePerNight}</td>
//             <td>{room.totalAmountInHotel}</td>
//             <td>{room.featured ? 'Yes' : 'No'}</td>
//             <td>{room.imageUrl === '' ? 'No' : 'Yes'}</td>
//             <td><Link to={`/employee/rooms/update/${room.id}`} className='update-link'>Update</Link></td>
//         </tr>
// )}
// </tbody>
// </table>