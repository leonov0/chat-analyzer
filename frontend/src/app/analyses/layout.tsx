import { AnalysesSidebar } from "@/components/analyses-sidebar";
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/ui/sidebar";

export default function Layout({ children }: { children: React.ReactNode }) {
  const analyses = [
    {
      id: "1",
      name: "Chat 1",
      created_at: new Date().toISOString(),
      updated_at: new Date().toISOString(),
    },
    {
      id: "2",
      name: "Chat 2",
      created_at: new Date().toISOString(),
      updated_at: new Date().toISOString(),
    },
  ];

  return (
    <SidebarProvider className="max-w-screen">
      <AnalysesSidebar analyses={analyses} />
      <SidebarInset className="space-y-4 overflow-auto p-4">
        <SidebarTrigger />
        {children}
      </SidebarInset>
    </SidebarProvider>
  );
}
