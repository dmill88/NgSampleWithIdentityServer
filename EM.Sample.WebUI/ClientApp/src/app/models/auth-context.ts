import { SimpleClaim } from './simple-claim';
import { Constants } from './../constants';

export class AuthContext {
  public claims: SimpleClaim[] = new Array<SimpleClaim>();
  public isAuthenticated: boolean;
  public userId: string;
  public name: string;

  public get isAdmin() {
    console.log('AuthContext.isAdmin called.');
    return this.isInRole(Constants.userRoles.admin);
    //return !!this.claims && !!this.claims.find(c =>
    //  c.type === 'role' && c.value.toLowerCase() === 'admin');
  }
  public get isBlogEditor() {
    return this.isInRole(Constants.userRoles.blogEditor);
  }

  public isInRole(role: string): boolean {
    console.log(`isInRole role = ${role}`);
    role = role.toLowerCase();
    let isInRole: boolean = false;
    let indexOfRole: number = this.claims.findIndex((c) => c.type === 'role' && (c.value.toLowerCase() === role || c.value.toLowerCase() == 'admin'));

    if (indexOfRole > -1) {// filteredRoles.length > 0) {
      isInRole = true;
      console.log(`indexOfRole = ${indexOfRole}`);
    }
    return isInRole;
  }

}

