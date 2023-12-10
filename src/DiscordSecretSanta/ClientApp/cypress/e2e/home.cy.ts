import { HomeRequest, HomeResponse } from "@request/home.request";

const homeUrl = new HomeRequest().url;

function setResponse(body: HomeResponse) {
  cy.intercept('GET', homeUrl, {
    statusCode: 200,
    body
  });
}

describe('Home', () => {
  it('should show configuration issues if they are any', () => {
    // ARRANGE
    setResponse({
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
    cy.hasComponent('page-home-no-config');
  });

  it('should show health issues if they are any', () => {
    // ARRANGE
    setResponse({
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
    cy.hasComponent('page-home-not-healthy');
  });

  it('should show login page if there aren\'t any admins and the user is not logged in', () => {
    // ARRANGE
    setResponse({
      configOk: true,
      admins: false,
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.hasComponent('page-home-no-admins');
  });

  it('should get the user to select the server if they are an admin', () => {
    // ARRANGE
    setResponse({
      configOk: true,
      admins: true,
      user: {
        isAdmin: true,
        name: 'Test User'
      }
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.hasComponent('page-home-select-server');
  });

  it('should give the user a message if there isn\'t an active campaign and they are not an admin', () => {
    // ARRANGE
    setResponse({
      configOk: true,
      admins: true,
      user: {
        isAdmin: false,
        name: 'Test User'
      },
      activeCampaign: undefined
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.hasComponent('page-home-not-admin-no-campaign');
  });

  it('should give the user a message if there isn\'t an active campaign and they are an admin', () => {
    // ARRANGE
    setResponse({
      configOk: true,
      admins: true,
      user: {
        isAdmin: true,
        name: 'Test User'
      },
      activeCampaign: undefined
    });

    // ACT
    cy.visit('/');

    // ASSERT
    cy.hasComponent('page-home-setup-campaign');
  });

  it('should give them a homepage of the campaign if there is an active one', () => {
    // ARRANGE
    setResponse({
      configOk: true,
      admins: true,
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
    cy.hasComponent('page-home-campaign-home');
  });
});
