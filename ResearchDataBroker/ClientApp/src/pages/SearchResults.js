import React, {useEffect, useState} from "react";
import Results from "../components/Results";
import {Link, useParams} from "react-router-dom";
import {Home} from "./Home";
import SearchComponent from "../components/SearchComponent";

const SearchResults = () => {
    let {item} = useParams()
    const [currentItem, setCurrentItem] = useState(item);

    useEffect(() => {
        setCurrentItem(item)
    }, [item]);
    
    return (
        <>
            <SearchComponent />
            <Results item={currentItem}/>
        </>
    )
}

export default SearchResults