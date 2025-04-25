export const APIRoutes = {
  analyses: "/api/analyses",
} as const;

export function getAnalysisRoute(id: string) {
  return `${APIRoutes.analyses}/${id}`;
}

export function getSendMessageRoute(id: string) {
  return `${getAnalysisRoute(id)}/messages`;
}
