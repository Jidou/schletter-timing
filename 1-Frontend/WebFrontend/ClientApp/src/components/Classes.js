import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Classes extends Component {
    static displayName = Classes.name;

    newClassCounter = -1;
    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
        this.handleAdd = this.handleAdd.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.state = { classes: [], loading: true };

        fetch('api/Class/GetAvailableClasses')
            .then(response => response.json())
            .then(data => {
                this.setState({ classes: data, loading: false });
            });
    }


    handleChange(classId, event) {
        var classes = this.state.classes;
        var index = classes.findIndex((x) => x.classId === classId);
        var value = event.target.value;
        var tmp = classes[index];

        tmp.className = value;
        classes[index] = tmp;

        this.setState({classes: classes});
    }


    handleBlur(classId) {
        var classes = this.state.classes;
        var index = classes.findIndex((x) => x.classId === classId);
        var cl = classes[index];

        if (cl.classId > 0) {
            this.updateClass(cl);
        } else {
            this.addClass(cl);
        }
    }


    handleAdd() {
        var newClass = {
            classId: this.newClassCounter,
            className: "",
        }

        this.newClassCounter--;

        var classes = this.state.classes;
        classes.push(newClass);

        this.setState({classes: classes});
    }


    addClass(cl) {
        fetch('api/Class/AddClass', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(cl)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({classes: data});
                toast("Class: " + cl.className + " added successfully");
            });
    }


    updateClass(cl) {
        fetch('api/Class/UpdateClass', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(cl)
        })
            .then(response => response.json())
            .then(data => {
                this.setState({classes: data});
                toast("Class: " + cl.className + " updated successfully");
            });
    }


    renderTable(classes) {
        return (
            <div>
                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Class</th>
                        </tr>
                    </thead>
                    <tbody>
                        {classes.map(cl =>
                            <tr key={cl.classId}>
                                <td>
                                    <input type="text" onChange={this.handleChange.bind(this, cl.classId)} onBlur={this.handleBlur.bind(this, cl.classId)} value={cl.className}></input>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderTable(this.state.classes);

        return (
            <div>
                <h1>Classes</h1>
                <form>
                    <div>
                        <button type="button" onClick={this.handleAdd} className="btn btn-primary">Add Class</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
