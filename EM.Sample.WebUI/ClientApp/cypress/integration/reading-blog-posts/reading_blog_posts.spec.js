context("Reading blog posts", () => {
  it('Reading blog posts', () => {
    expect(true).to.equal(true);
  });

  it('Load Home Page', () => {
    cy.visit("/");
    cy.contains('Sample WebUI');
  });

  it('Load blog', () => {
    cy.get('div.blog-list > div:nth-child(1) > a').click();
    cy.contains('Posts');
    cy.wait(2000);
  });

  it('Load blog post and go back to blog', () => {
    cy.get('#divBlogPosts > div:nth-child(1) > a').click();
    cy.wait(2000);
    cy.contains('Return to Blog Home').click();
    cy.contains('Posts');
  });

  it('Load blog post and go back to blog', () => {
    cy.get('#divBlogPosts > div:nth-child(1) > a').click();
    cy.wait(2000);
    cy.contains('Return to Blog Home').click();
    cy.contains('Posts');
    cy.wait(2000);
  });

  it('Click on Home link', () => {
    cy.contains('Sample WebUI').click();
  });

});
