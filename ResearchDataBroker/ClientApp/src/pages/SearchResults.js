import React from "react";
import Results from "../components/Results";
import {Link, useParams} from "react-router-dom";
import {Home} from "./Home";

const SearchResults = () => {
    // let {item} = useParams()
    let {item} = useParams()
    
    return (
        <main>
            <h3>Results for {item}:</h3>
            <h3 className={'button'}><Link to={'/'}>Search</Link></h3>
            <Results item={item}/>
        </main>
    )
}

export default SearchResults