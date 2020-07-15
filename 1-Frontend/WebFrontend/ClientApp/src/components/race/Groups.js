import React, { Component } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class RaceGroups extends Component {
    static displayName = RaceGroups.name;

    newGroupsCounter = -1;


    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleAddGroup = this.handleAddGroup.bind(this);

        this.state = { groups: [], loading: true };

        fetch('api/Group/')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data, loading: false, activePage: 1 });
            });
    }


    handleChange(groupId, event) {
        var groups = this.state.groups;
        var index = groups.findIndex((x) => x.groupId === groupId);
        var target = event.target.id;
        var value = event.target.value;
        var tmp = groups[index];

        if (target === "groupname") {
            tmp.groupname = value;
        } else if (target === "groupclass") {
            tmp.class = value;
        } else {
            return;
        }

        if (tmp.toAdd === false && tmp.toDelete === false) {
            tmp.toUpdate = true;
        }

        groups[index] = tmp;

        this.setState({
            groups: groups
        });
    }


    handleAddGroup() {
        var newGroup = {
            groupId: this.newGroupsCounter,
            groupname: "",
            class: "",
            toAdd: true,
        }

        this.newGroupsCounter--;

        var groups = this.state.groups;
        groups.push(newGroup);

        this.setState({
            groups: groups
        });
    }


    handleSubmit(event) {
        fetch('api/Group/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.groups)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data });
            });


        event.preventDefault();
        toast("Groups saved successfully");
    }


    showGroupDetails() {

    }


    renderGroupsTable(groups) {
        return (
            <div>
                <table className="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>Groupname</th>
                            <th>Class</th>
                            <th>Participant 1</th>
                            <th>Participant 2</th>
                        </tr>
                    </thead>
                    <tbody>
                        {groups.map(group =>
                            <tr key={group.groupId}>
                                <td>
                                    <input type="text" id="groupname" onChange={this.handleChange.bind(this, group.groupId)} value={group.groupname}></input>
                                </td>
                                <td>
                                    <input type="text" id="groupclass" onChange={this.handleChange.bind(this, group.groupId)} value={group.class}></input>
                                </td>
                                <td>{group.participant1FullName}</td>
                                <td>{group.participant2FullName}</td>
                            </tr>
                        )}
                    </tbody>
                </table>

                {/* <div>
                    <nav aria-label="Page navigation example">
                        <ul className="pagination">
                            <li className="page-item"><a className="page-link" href="#">Previous</a></li>
                            <li className="page-item"><a className="page-link" href="#">1</a></li>
                            <li className="page-item"><a className="page-link" href="#">2</a></li>
                            <li className="page-item"><a className="page-link" href="#">3</a></li>
                            <li className="page-item"><a className="page-link" href="#">Next</a></li>
                        </ul>
                    </nav>
                </div> */}
            </div>
        );
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderGroupsTable(this.state.groups);

        return (
            <div>
                <h1>Race Groups</h1>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" onClick={this.handleAddGroup} disabled={this.dirty} className="btn btn-primary">Add Group</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
