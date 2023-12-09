describe('Setup', () => {
  it('Has no discord configuration so prompt the user to update it', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Setup');
    cy.contains('Your configuration details are incorrect, please view the ReadMe Setup Guide for more information');
  });

  it('Has no users so should prompt for user to login', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Setup');
    cy.contains('To setup Discord Secret Santa, you will need to login. This will set your account as an administrator and you can continue the setup process');
  });

  it('Has a user but no campaign and it is the user viewing it so should prompt the user to enter details', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Setup');
    cy.contains('You have not setup a campaign, please enter your Discord Secret Santa campaign details below');
  });

  it('Has a user but its not the currently viewing user so should show a waiting message', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Hold your horses! The campaign isn\'t setup yet, please contact AdminUserName on ServerName if you are expecting access');
  });
})
