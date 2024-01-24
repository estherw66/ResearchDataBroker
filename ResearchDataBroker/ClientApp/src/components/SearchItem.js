import React, {useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import axios from "axios";

const SearchItem = () => {
    
    const navigate = useNavigate()
    // const {item} = useParams()
    
    const [searchQuery, setSearchQuery] = useState("")
    // const [files, setFiles] = useState([])
    // const url = `api/index/files/${searchQuery}`

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        navigate(`search/${searchQuery}`)
    }
    
    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input type={'text'} value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} required />
                <button>Zoek</button>
            </form>
        </div>
    )
}

export default SearchItem