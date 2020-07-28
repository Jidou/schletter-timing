import React, { Component } from 'react';
import { Table, Form, Row, Col } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Result extends Component {
    static displayName = Result.name;

    dirty = false;


    constructor(props) {
        super(props);

        this.state = { race: [], loading: true };

        fetch('api/Result/LoadResult')
            .then(response => response.json())
            .then(data => {
                this.setState({ race: data, loading: false });
            });
    }


    renderTable(race) {
        return (
            <div>
                <Form>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            Racename
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.titel} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            RaceType
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.raceType} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            Place
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.place} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            Date
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.date} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            Start Time
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.startTime} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            Judge
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.judge} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formPlaintextEmail">
                        <Form.Label column sm="2">
                            Timing Tool
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={this.state.race.timingTool} />
                        </Col>
                    </Form.Group>
                </Form>
                <Table striped hover>
                    <thead>
                        <tr>
                            <th>Groupname</th>
                            <th>Start Number</th>
                            <th>Runner</th>
                            <th>Cyclist</th>
                            <th>Finish Time</th>
                            <th>Time Taken</th>
                        </tr>
                    </thead>
                    <tbody>
                        {race.groups.map(group =>
                            <tr key={group.groupId}>
                                <td>{group.groupname}</td>
                                <td>{group.startnumber}</td>
                                <td>{group.participant1Name}</td>
                                <td>{group.participant2Name}</td>
                                <td>{group.finishTime}</td>
                                <td>{group.timeTaken}</td>
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
            : this.renderTable(this.state.race);

        return (
            <div>
                <h1>Result</h1>
                <form onSubmit={this.handleSubmit}>
                    {/* <div>
                        <button type="button" onClick={this.handleAssign} className="btn btn-primary">Assign</button>
                        <button type="button" onClick={this.handleLoad} className="btn btn-primary">Get Times</button>
                    </div> */}
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
