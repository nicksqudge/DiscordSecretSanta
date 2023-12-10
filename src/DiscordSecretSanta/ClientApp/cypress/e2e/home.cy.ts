import { HomeResponse } from "../../src/app/pages/home/home.service";

const homeUrl = '/api/home'

function setResponse(statusCode: number, body: HomeResponse) {
  cy.intercept('GET', homeUrl, {
    statusCode: statusCode,
    body
  });
}

describe('Home', () => {
  it('should show configuration issues if they are any', () => {
    // ARRANGE
    setResponse(500, {
      configOk: false,
      configDetail: [
        {
          key: 'no_config',
          healthy: false,
          reason: 'not_configured'
        }
      ],
      admins: false,
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-no-config]').should('exist');
  });

  it('should show health issues if they are any', () => {
    // ARRANGE
    setResponse(500, {
      configOk: false,
      configDetail: [
        {
          key: 'database',
          healthy: false,
          reason: 'no_connection'
        }
      ],
      admins: false,
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-not-healthy]').should('exist');
  });

  it('should show login page if there aren\'t any admins and the user is not logged in', () => {
    // ARRANGE
    setResponse(200, {
      configOk: true,
      admins: false,
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-no-admins]').should('exist');
  });

  it('should get the user to select the server if they are an admin', () => {
    // ARRANGE
    setResponse(200, {
      configOk: true,
      admins: false,
      user: {
        isAdmin: true,
        name: 'Test User'
      }
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-select-server]').should('exist');
  });

  it('should give the user a message if they setup is not complete and they are not an admin', () => {
    // ARRANGE
    setResponse(200, {
      configOk: true,
      admins: false,
      user: {
        isAdmin: false,
        name: 'Test User'
      }
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-not-admin-setup-incomplete]').should('exist');
  });

  it('should give the user a message if there isn\'t an active campaign and they are not an admin', () => {
    // ARRANGE
    setResponse(200, {
      configOk: true,
      admins: false,
      user: {
        isAdmin: false,
        name: 'Test User'
      },
      activeCampaign: undefined
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-not-admin-no-campaign]').should('exist');
  });

  it('should give the user a message if there isn\'t an active campaign and they are an admin', () => {
    // ARRANGE
    setResponse(200, {
      configOk: true,
      admins: false,
      user: {
        isAdmin: true,
        name: 'Test User'
      },
      activeCampaign: undefined
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-setup-campaign]').should('exist');
  });

  it('should give them a homepage of the campaign if there is an active one', () => {
    // ARRANGE
    setResponse(200, {
      configOk: true,
      admins: false,
      user: {
        isAdmin: true,
        name: 'Test User'
      },
      activeCampaign: {
        name: 'Test Campaign'
      }
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.get('[data-test=home-campaign]').should('exist');
  });
});
