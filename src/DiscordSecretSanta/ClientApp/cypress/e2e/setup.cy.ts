describe('Setup', () => {
  it('Has no users so should prompt for user to login', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Setup');
    cy.contains('To setup Discord Secret Santa, you will need to login. This will set your account as an administrator and you can continue the setup process');
  });

  it('Has a user but no campaign so should prompt the user to enter details', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Setup');
    cy.contains('You have not setup a campaign, please enter your Discord Secret Santa campaign details below');
  });

  it('Has a campaign setup so should show the campaign homepage', () => {
    // ARRANGE

    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('CampaignHome Works!');
  });
})
