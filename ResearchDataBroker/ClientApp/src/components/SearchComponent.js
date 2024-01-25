import React, {useEffect, useState} from "react";
import {useLocation, useNavigate, useParams} from "react-router-dom";
import axios from "axios";
import {Nav} from "reactstrap";

const SearchComponent = () => {
    
    const location = useLocation()
    const navigate = useNavigate()
    const [searchQuery, setSearchQuery] = useState("")

    useEffect(() => {
        
    }, []);


    const handleSubmit = async (e) => {
        e.preventDefault();

        if (location.pathname === '/') {
            navigate(`/search/${searchQuery}`);
        } else {
            navigate(`/search/${searchQuery}`);
        }
    }

        return (
        <>
            <div className={'search-container'}>
                <div className={'search-box'}>
                    <h1 className={'title'}>Zoek data:</h1>
                    <form onSubmit={handleSubmit}>
                        <input className={'search-bar'} type={'text'} value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} placeholder={'Item...'} required />
                        <button className={'search-btn'}>Search</button>
                    </form>
                </div>
                <hr/>
            </div>
        </>
    )
}

export default SearchComponent