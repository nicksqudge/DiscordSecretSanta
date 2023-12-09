describe('Setup', () => {
  it('Has no users so should prompt for user to login', () => {
    // ACT
    cy.visit('/');

    // ASSERT
    cy.contains('Setup');
    cy.contains('To setup Discord Secret Santa, you will need to login. This will set your account as an administrator and you can continue the setup process');
  })
})
