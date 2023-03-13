import { SPATemplatePage } from './app.po';

describe('SPA App', function() {
  let page: SPATemplatePage;

  beforeEach(() => {
    page = new SPATemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
