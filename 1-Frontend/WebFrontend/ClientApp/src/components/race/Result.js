import React, { Component } from 'react';
import { Table, Form, Row, Col } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';
import moment from 'moment';

import { generatePdf, generatePdfWithFilter, calcuateTimeDiffs } from './../../util/pdfGenerator';

import 'react-toastify/dist/ReactToastify.css';


export class Result extends Component {
    static displayName = Result.name;

    dirty = false;


    constructor(props) {
        super(props);

        this.handleGenerate = this.handleGenerate.bind(this);
        this.toggleGrouping = this.toggleGrouping.bind(this);
        this.handleSelect = this.handleSelect.bind(this);

        this.state = { race: [], classes: [], groupByClass: false, loading: true };

        fetch('api/Result/LoadResult')
            .then(response => response.json())
            .then(data => {
                data.startTime = this.convertStartTimeToMoment(data.startTime);
                this.convertFinishTimesToMoment(data.groups);
                calcuateTimeDiffs(data.groups);
                this.setState({ race: data });
                fetch('api/Result/GetAllClasses')
                    .then(response => response.json())
                    .then(data => {
                        this.setState({ classes: data, loading: false });
                    });
            });
    }


    convertStartTimeToMoment(startTime) {
        return moment(startTime);
    }


    convertFinishTimesToMoment(groups, startTime) {
        groups.forEach(group => {
            group.timeTaken = moment("0001-01-02T" + group.timeTaken);
        });
    }

    renderForm(race){
        return (
            <div>
                <Form>
                    <Form.Group as={Row} controlId="formRacename">
                        <Form.Label column sm="2">
                            Racename
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.titel} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formRaceType">
                        <Form.Label column sm="2">
                            RaceType
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.raceType} />
                        </Col>
                    </Form.Group>
                    {/* <Form.Group as={Row} controlId="formPlace">
                        <Form.Label column sm="2">
                            Place
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.place} />
                        </Col>
                    </Form.Group> */}
                    <Form.Group as={Row} controlId="formDate">
                        <Form.Label column sm="2">
                            Date
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.date} />
                        </Col>
                    </Form.Group>
                    <Form.Group as={Row} controlId="formStartTime">
                        <Form.Label column sm="2">
                            Start Time
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.startTime.format("YYYY MM D HH:mm:ss.SSS")} />
                        </Col>
                    </Form.Group>
                    {/* <Form.Group as={Row} controlId="formJudge">
                        <Form.Label column sm="2">
                            Judge
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.judge} />
                        </Col>
                    </Form.Group> */}
                    {/* <Form.Group as={Row} controlId="formTimingTool">
                        <Form.Label column sm="2">
                            Timing Tool
                        </Form.Label>
                        <Col sm="10">
                            <Form.Control plaintext readOnly defaultValue={race.timingTool} />
                        </Col>
                    </Form.Group> */}
                </Form>
            </div>
        );
    }


    renderGroupedTable(groups, classes) {
        return (
            <div>
                {classes.map(c =>
                    <div key={c.cn}>
                        <h2>{c.cn}</h2>
                        <Table striped hover>
                            {this.tableHeaders()}
                            <tbody>
                                {groups.filter(x => x.groupClass == c.cn).map(group =>
                                    this.tableRow(group)
                                )}
                            </tbody>
                        </Table>
                    </div>
                )}
            </div>
        );
    }


    renderTable(groups) {
        return (
            <div>
                <Table striped hover>
                    {this.tableHeaders()}
                    <tbody>
                        {groups.map(group =>
                            this.tableRow(group)
                        )}
                    </tbody>
                </Table>
            </div>
        );
    }


    tableHeaders() {
        return (
            <thead>
                <tr>
                    <th></th>
                    <th>Groupname</th>
                    <th>Start Number</th>
                    <th>Runner</th>
                    <th>Cyclist</th>
                    <th>Time Taken</th>
                    <th>Time Diff</th>
                </tr>
            </thead>
        );        
    }


    tableRow(group) {
        return (
            <tr key={group.groupId}>
                <td>
                    <input type="checkbox" className="form-check-input" onClick={this.handleSelect.bind(this, group.groupId)} checked={group.selected}></input>    
                </td>
                <td>{group.groupname}</td>
                <td>{group.startnumber}</td>
                <td>{group.participant1Name}</td>
                <td>{group.participant2Name}</td>
                <td>{group.timeTaken.format('HH:mm:ss.SSS')}</td>
                <td>{group.timeDiff.format('HH:mm:ss.SSS')}</td>
            </tr>
        );
    }


    handleGenerate() {
        let groups = this.state.race.groups.filter(x => x.selected);

        if (this.state.groupByClass) {
            var filterFunction = function (groups, c) { return groups.filter(x => x.groupClass == c.cn)}
            generatePdfWithFilter(this.state.race, groups, filterFunction, this.state.classes);
        } else {
            generatePdf(this.state.race, groups);
        }
    }


    toggleGrouping() {
        let race = this.state.race;
        let groups = race.groups;

        if (!this.state.groupByClass) {
            //filtered
            this.state.classes.forEach(c => {
                let tmp = groups.filter(x => x.groupClass == c.cn);
                calcuateTimeDiffs(tmp);
            })
        } else {
            //non filtered
            calcuateTimeDiffs(groups);
        }

        race.groups = groups;

        this.setState({
            race: race,
            groupByClass: !this.state.groupByClass
        });
    }


    handleSelect(groupId, event) {
        let race = this.state.race;
        let groups = race.groups;
        let index = groups.findIndex((x) => x.groupId === groupId);
        let value = event.target.checked;
        let tmp = groups[index];

        tmp.selected = value;
        groups[index] = tmp;
        race.groups = groups;

        this.setState({
            race: race
        });
    }


    render() {
        let contents;

        if (this.state.loading) {
            contents = <p><em>Loading...</em></p>;
        } else if (this.state.groupByClass) {
            let form = this.renderForm(this.state.race);
            let table = this.renderGroupedTable(this.state.race.groups, this.state.classes);
            contents = <div>{form}{table}</div>;
        } else {
            let form = this.renderForm(this.state.race);
            let table = this.renderTable(this.state.race.groups);
            contents = <div>{form}{table}</div>;
        }

        return (
            <div>
                <h1>Result</h1>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <button type="button" onClick={this.handleGenerate} className="btn btn-primary">Generate</button>
                        <button type="button" onClick={this.toggleGrouping} className="btn btn-primary">Toggle Grouping</button>
                    </div>
                    {contents}
                </form>
                <ToastContainer />
            </div>
        );
    }
}
