import React, { Component } from 'react';
import IndexDataset from "../components/IndexDataset";
import SearchItem from "../components/SearchItem";


export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
            <h3>Zoek data bestanden:</h3>
          <SearchItem />
          {/*<IndexDataset />*/}
      </div>
    );
  }
}
