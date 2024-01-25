import React, { Component } from 'react';
import IndexDataset from "../components/IndexDataset";
import SearchComponent from "../components/SearchComponent";


export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
        <>
            <SearchComponent />
        </>
    );
  }
}
