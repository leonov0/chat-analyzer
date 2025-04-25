"use client";

import { AnalysesSidebar } from "@/components/analyses-sidebar";
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/ui/sidebar";
import { useAnalyses } from "@/features/analyses";

export default function Layout({ children }: { children: React.ReactNode }) {
  const { analyses, isLoading } = useAnalyses();

  return (
    <SidebarProvider className="max-w-screen">
      <AnalysesSidebar analyses={analyses} isLoading={isLoading} />
      <SidebarInset className="space-y-4 overflow-auto p-4">
        <SidebarTrigger />
        {children}
      </SidebarInset>
    </SidebarProvider>
  );
}
