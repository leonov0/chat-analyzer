import { FileText, LogOut } from "lucide-react";
import Link from "next/link";

import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar";
import { Skeleton } from "@/components/ui/skeleton";
import { type AnalysisPreview, CreateAnalysisForm } from "@/features/analyses";
import { getAnalysesRoute, routes } from "@/lib/routes";

import { ModeToggle } from "./mode-toggle";
import { Button } from "./ui/button";

export function AnalysesSidebar({
  analyses = [],
  isLoading,
}: {
  analyses?: AnalysisPreview[];
  isLoading: boolean;
}) {
  return (
    <Sidebar>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Analyses</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {analyses
                .sort(
                  (a, b) =>
                    new Date(b.updated_at).getTime() -
                    new Date(a.updated_at).getTime(),
                )
                .map((chat) => (
                  <SidebarMenuItem key={chat.id}>
                    <SidebarMenuButton asChild>
                      <Link href={getAnalysesRoute(chat.id)}>{chat.name}</Link>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                ))}

              {isLoading && (
                <>
                  <Skeleton className="h-8" />
                  <Skeleton className="h-8" />
                  <Skeleton className="h-8" />
                </>
              )}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      <SidebarFooter>
        <Dialog>
          <DialogTrigger asChild>
            <Button>
              <FileText />
              Analyze New Chat
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Analyze New Chat</DialogTitle>
              <DialogDescription>
                Submit a new chat for analysis. If you do not know how to do
                this, please refer to the documentation.
              </DialogDescription>
              <CreateAnalysisForm />
            </DialogHeader>
          </DialogContent>
        </Dialog>

        <div className="grid grid-cols-[1fr_auto] gap-2">
          <Button variant="outline" asChild>
            <Link href={routes.logout}>
              <LogOut />
              Log Out
            </Link>
          </Button>
          <ModeToggle />
        </div>
      </SidebarFooter>
    </Sidebar>
  );
}
