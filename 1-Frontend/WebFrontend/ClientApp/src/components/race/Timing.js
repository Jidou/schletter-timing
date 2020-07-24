import React, { Component } from 'react';
import { Table, Container, Row, Col } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Timing extends Component {
    static displayName = Timing.name;

    dirty = false;


    constructor(props) {
        super(props);

        this.handleLoad = this.handleLoad.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);

        this.state = { groups: [], times: [], loading: true };

        fetch('api/Timing/LoadTimingValues')
            .then(response => response.json())
            .then(data => {
                this.setState({ groups: data });
            });

        fetch('api/Timing/GetTimes')
            .then(response => response.json())
            .then(data => {
                this.setState({ times: data, loading: false });
            });
    }


    renderTable(groups, times) {
        return (
            <div>
                <Container>
                    <Row>
                        <Col>
                            <Table striped hover>
                                <thead>
                                    <tr>
                                        <th>Groupname</th>
                                        <th>Start Number</th>
                                        {/* <th>Runner</th>
                                        <th>Cyclist</th> */}
                                        <th>Finish Time</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {groups.map(group =>
                                        <tr key={group.groupId}>
                                            <td>{group.groupname}</td>
                                            <td>{group.startnumber}</td>
                                            {/* <td>{group.participant1Time}</td>
                                            <td>{group.participant2Time}</td> */}
                                            <td>{group.finishTime}</td>
                                        </tr>
                                    )}
                                </tbody>
                            </Table>
                        </Col>
                        <Col>
                            <Table striped hover>
                                <thead>
                                    <tr>
                                        <th>Start Number</th>
                                        <th>Time</th>
                                        <th>Measurement Number</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {times.map(time =>
                                        <tr key={time.measurementNumber}>
                                            <td>
                                                <input type="text" onChange={this.handleChange.bind(this, time.measurementNumber)} value={time.startNumber}></input>
                                            </td>
                                            <td>{time.time}</td>
                                            <td>{time.measurementNumber}</td>
                                        </tr>
                                    )}
                                </tbody>
                            </Table>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
    }


    handleLoad() {
        fetch('api/Timing/GetTimes')
            .then(response => response.json())
            .then(data => {
                this.setState({ times: data });
            });
    }


    handleSubmit() {
        fetch('api/Timing/Save', {
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


        toast("Times saved successfully");
    }


    handleChange(measurementNumber, event) {
        var times = this.state.times;
        var index = times.findIndex((x) => x.measurementNumber === measurementNumber);
        var value = event.target.value;
        var tmp = times[index];

        tmp.startNumber = value;
        times[index] = tmp;

        this.setState({
            times: times
        });
    }


    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderTable(this.state.groups, this.state.times);

        return (
            <div>
                <h1>Timing</h1>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" onClick={this.handleLoad} className="btn btn-primary">Get Times</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
