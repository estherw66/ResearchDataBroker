import React from 'react'

const SearchPage = () => {

    const handleSubmit = (e) => {
        e.preventDefault()

    }
    
    return (
        <div>
            Search items:
            <form onSubmit={handleSubmit}>
                <input id='itemName'/>
            </form>
        </div>
    )
}

export default SearchPage