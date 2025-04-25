import useSWR from "swr";

import { axiosInstance } from "@/lib/axios";

import { getAnalysisRoute } from "./api-routes";
import type { Analysis } from "./types";

async function fetchAnalysis(id: string) {
  const { data } = await axiosInstance.get<Analysis>(getAnalysisRoute(id));

  return data;
}

export function useAnalysis(id: string) {
  const { data: analysis, isLoading } = useSWR(getAnalysisRoute(id), () =>
    fetchAnalysis(id),
  );

  return {
    analysis,
    isLoading,
  };
}
