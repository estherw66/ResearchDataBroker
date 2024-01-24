import React, {useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";
import axios from "axios";

const Results = ({item}) => {
    
    // let {item} = useParams()
    const URL = `/api/index/files/${item}`
    
    const [files, setFiles] = useState([])
    
    const getFiles = () => {
        axios.get(URL)
            .then(res => {
                setFiles(res.data.files)
                console.log(res.data.files)
            })
            .catch(err => {
                console.log(err)
            })
    }

    useEffect(() => {
        getFiles()
    }, []);
    
    return (
        <main>
            Total found: {files.length}
            {files.length > 0 ? (
                <>
                    <table className={'table'}>
                        <thead>
                        <tr>
                            <th>Id</th>
                            <th>Filename</th>
                            <th>Parent id</th>
                            <th>Item</th>
                            {/*<th>Download</th>*/}
                        </tr>
                        </thead>
                        <tbody>
                        {files?.map(
                            file =>
                                <tr key={file.id}>
                                    <td>{file.id}</td>
                                    <td>{file.filename}</td>
                                    <td>{file.parentId}</td>
                                    <td>{file?.itemNames}</td>
                                    {/*<td><button><a href={file.link}>Download</a> </button></td>*/}
                                </tr>
                        )}
                        </tbody>
                    </table>
                </>
            ) : (
                <>No results</>
            )}
        </main>
    )
}

export default Results