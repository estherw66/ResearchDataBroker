import React, { Component } from 'react';
import IndexDataset from "../components/IndexDataset";


export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
            <h1>Hello, world!</h1>
        <p>Welkom bij de indexeer app :)</p>
          <p>Voer een url van een Dataverse dataset in om deze te indexeren:</p>
          <IndexDataset />
      </div>
    );
  }
}
