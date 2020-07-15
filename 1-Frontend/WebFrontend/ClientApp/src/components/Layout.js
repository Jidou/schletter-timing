import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Navbar, Nav, NavDropdown } from 'react-bootstrap';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <Navbar bg="primary" variant="dark">
          <Navbar.Brand href="/">Schletter Timing</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav" className="justify-content-end">
            <Nav className="mr-auto">
              <Nav.Link href="/races">Races</Nav.Link>
              <Nav.Link href="/groups">Groups</Nav.Link>
              <Nav.Link href="/participants">Participants</Nav.Link>
              <NavDropdown title="Race" id="basic-nav-dropdown">
                <NavDropdown.Item href="/race/overview">Overview</NavDropdown.Item>
                <NavDropdown.Item href="/race/groups">Groups</NavDropdown.Item>
                <NavDropdown.Item href="/race/participants">Participants</NavDropdown.Item>
              </NavDropdown>
            </Nav>
          </Navbar.Collapse>
        </Navbar>

        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
