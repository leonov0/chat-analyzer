export const routes = {
  login: "/login",
  register: "/register",
  logout: "/logout",
  analyses: "/analyses",
  home: "/",
  terms: "/terms",
  privacy: "/privacy",
};

export function getAnalysesRoute(id: string) {
  return `/analyses/${id}`;
}
