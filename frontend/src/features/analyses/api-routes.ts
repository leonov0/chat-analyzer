export const APIRoutes = {
  analyses: "/api/analyses",
} as const;

export function getAnalysisRoute(id: string) {
  return `${APIRoutes.analyses}/${id}`;
}
