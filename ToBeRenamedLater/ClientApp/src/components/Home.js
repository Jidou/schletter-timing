import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>Hello, world!</h1>
                <p>Welcome to your new single-page application, built with:</p>
                <ul>
                    <li>First check if all <a href='./groups'>Groups</a> are present</li>
                    <li>Then check the <a href='./Participants'>Participants</a>, and assign them to their groups</li>
                    <li>After all this is done, you can create a new <a href='./Race'>Race</a></li>
                </ul>
            </div>
        );
    }
}
