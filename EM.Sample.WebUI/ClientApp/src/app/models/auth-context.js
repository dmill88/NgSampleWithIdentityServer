import { Constants } from './../constants';
export class AuthContext {
    constructor() {
        this.claims = new Array();
    }
    get isAdmin() {
        console.log('AuthContext.isAdmin called.');
        return this.isInRole(Constants.userRoles.admin);
        //return !!this.claims && !!this.claims.find(c =>
        //  c.type === 'role' && c.value.toLowerCase() === 'admin');
    }
    get isBlogEditor() {
        return this.isInRole(Constants.userRoles.blogEditor);
    }
    isInRole(role) {
        console.log(`isInRole role = ${role}`);
        role = role.toLowerCase();
        let isInRole = false;
        let indexOfRole = this.claims.findIndex((c) => c.type === 'role' && (c.value.toLowerCase() === role || c.value.toLowerCase() == 'admin'));
        if (indexOfRole > -1) { // filteredRoles.length > 0) {
            isInRole = true;
            console.log(`indexOfRole = ${indexOfRole}`);
        }
        return isInRole;
    }
}
//# sourceMappingURL=auth-context.js.map