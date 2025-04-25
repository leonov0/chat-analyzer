export const routes = {
  login: "/login",
  register: "/register",
  logout: "/logout",
  chats: "/chats",
  home: "/",
  terms: "/terms",
  privacy: "/privacy",
};

export function getChatRoute(id: string) {
  return `/chats/${id}`;
}
